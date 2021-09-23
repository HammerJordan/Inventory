using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Inventory.Core;
using Microsoft.Extensions.Configuration;

namespace WebScraping
{
    public class ProductUpdateRunner
    {
        //TODO Refactor this into a helper SQL QUERY class
        private const string PRODUCT_QUERY_BY_NAME =
            @"select *
                                    from Product
                                    where Name = @name";

        private const string PRODUCT_INSERT_NEW = @"INSERT 
                                                    INTO Product                              
                                                    (Name, Description, UPC, Cost, Unit, URL, LastUpdated,ImageHref)
                                                    VALUES                                           
                                                    (@Name, @Description, @UPC, @Cost, @Unit, @URL,@NowToString,@ImageHref)";

        private const string PRODUCT_UPDATE = @"Update Product
                                                set
                                                    Description = @Description,
                                                    UPC = @UPC,
                                                    Cost = @Cost,
                                                    URL = @URL,
                                                    LastUpdated = @NowToString,
                                                    ImageHref = @ImageHref
                                                where ID = @ID";



        public float EstPercentDone { get; private set; }

        private readonly WebPageLoader pageLoader;
        private readonly ProductScraper productScraper;
        private readonly ISqlLiteDataAccess dataAccess;
        private readonly IConfiguration config;

        public ProductUpdateRunner(WebPageLoader pageLoader,
            ProductScraper productScraper,
            ISqlLiteDataAccess dataAccess,
            IConfiguration config)
        {
            this.pageLoader = pageLoader;
            this.productScraper = productScraper;
            this.dataAccess = dataAccess;
            this.config = config;
        }

        public async Task RunProductUpdate()
        {
            var entryUrl = config["EntryUrl"];

            var directoryTree = new DirectoryTreeModel() { RootURL = entryUrl };
            var page = await pageLoader.GetWebPageAsync(entryUrl);

            EstPercentDone = .01f;

            var categoryLinks = productScraper.GetCategoriesLinks(page, GetIgnoreCategories());
            directoryTree.Nodes.AddRange(categoryLinks);

            foreach (var categoryLink in directoryTree.Nodes)
                await productScraper.WalkCategoryGroups(categoryLink, directoryTree);

            EstPercentDone = .1f;

            var leafNodes = directoryTree.GetLeafNodes();

            var productLinks = new List<IElement>();
            int count = 0;

            foreach (var node in leafNodes)
            {
                count++;
                EstPercentDone = (float)count / leafNodes.Count;
                page = await pageLoader.GetWebPageAsync(directoryTree.GetUrlFromHref(node.Url));
                productLinks.Clear();

                await GetAllProductIElementsInCategory(productLinks, page, directoryTree);

                Parallel.ForEach(productLinks, (productLink) =>
                {
                    var model = productScraper.GetProductModel(productLink);
                    model.URL = directoryTree.GetUrlFromHref(model.URL);

                    var parent = node.GetRootNode();
                    var groups = node.GetBranchNodesFromParent();

                    model.Category = parent.Category;
                    model.Group = groups
                        .Select(x => x.Category)
                        .Aggregate((c, n) => c + "\\" + n);

                    var query = dataAccess.LoadData<ProductModel, object>(PRODUCT_QUERY_BY_NAME,
                        new { name = model.Name });

                    string sqlSave;
                    if (query == null || query.Count == 0)
                        sqlSave = PRODUCT_INSERT_NEW;
                    else
                    {
                        sqlSave = PRODUCT_UPDATE;
                        model.ID = query.First().ID;
                    }

                    dataAccess.SaveData(sqlSave, model);

                    query = dataAccess.LoadData<ProductModel, object>
                        (PRODUCT_QUERY_BY_NAME, new { name = model.Name });

                });
            }
        }



        private async Task GetAllProductIElementsInCategory(List<IElement> productLinks,
            IHtmlDocument page,
            DirectoryTreeModel directoryTree)
        {
            while (true)
            {
                productLinks.AddRange(productScraper.GetProductLinksFromPage(page));

                var next = productScraper.GetNextProductLinkPage(page);
                if (next is null)
                    break;
                page = await pageLoader.GetWebPageAsync(directoryTree.GetUrlFromHref(next));
            }
        }

        private string[] GetIgnoreCategories()
        {
            return config
                .GetSection("IgnoreProducts")
                .GetChildren()
                .Select(x => x.Value)
                .ToArray();
        }
    }
}