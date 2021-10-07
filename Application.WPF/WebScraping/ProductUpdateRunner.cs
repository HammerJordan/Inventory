using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Application.Core.Models.Product.Queries;
using Application.WPF.WebScraping.Common;
using Microsoft.Extensions.Configuration;

namespace Application.WPF.WebScraping
{
    public class ProductUpdateRunner : IProductUpdateRunner
    {
        private readonly WebPageLoader pageLoader;
        private readonly ProductScraper productScraper;
        private readonly IConfiguration config;
        private readonly IProductModelQuery _modelQuery;

        public string LastItemScraped { get; private set; }
        public int NumberOfItemsScraped { get; private set; }
        public float PercentDone { get; private set; }
        public bool IsDone { get; private set; }

        public ProductUpdateRunner(WebPageLoader pageLoader,
            ProductScraper productScraper,
            IConfiguration config,
            IProductModelQuery modelQuery)
        {
            this.pageLoader = pageLoader;
            this.productScraper = productScraper;
            this.config = config;
            _modelQuery = modelQuery;
        }

        public async Task RunProductUpdateAsync(Action callBack = null)
        {
            var entryUrl = config["EntryUrl"];

            var directoryTree = new DirectoryTreeModel() { RootURL = entryUrl };
            var page = await pageLoader.GetWebPageAsync(entryUrl);

            PercentDone = .01f;

            var categoryLinks = productScraper.GetCategoriesLinks(page, GetIgnoreCategories());
            directoryTree.Nodes.AddRange(categoryLinks);

            foreach (var categoryLink in directoryTree.Nodes)
                await productScraper.WalkCategoryGroups(categoryLink, directoryTree);

            var leafNodes = directoryTree.GetLeafNodes();

            var productLinks = new List<IElement>();
            int count = 0;
            

            foreach (var node in leafNodes)
            {
                count++;
                PercentDone = (float)count / leafNodes.Count;
                callBack?.Invoke();

                page = await pageLoader.GetWebPageAsync(directoryTree.GetUrlFromHref(node.Url));
                productLinks.Clear();

                await GetAllProductIElementsInCategory(productLinks, page, directoryTree);

                foreach (var productLink in productLinks)
                {

                        var model = productScraper.GetProductModel(productLink);
                        model.URL = directoryTree.GetUrlFromHref(model.URL);

                        var parent = node.GetRootNode();
                        var groups = node.GetBranchNodesFromParent();

                        var product = await _modelQuery.FindByNameAsync(model.Name);
                        if (product is null)
                           await _modelQuery.CreateAsync(model);
                        else
                        {
                            model.ID = product.ID;
                            await _modelQuery.UpdateAsync(model);
                        }

                        LastItemScraped = model.Name;
                        NumberOfItemsScraped++;
                    
                    callBack?.Invoke();
                }

                // Parallel.ForEach(productLinks, (productLink) =>
                // {
                //     var model = productScraper.GetProductModel(productLink);
                //     model.URL = directoryTree.GetUrlFromHref(model.URL);
                //
                //     var parent = node.GetRootNode();
                //     var groups = node.GetBranchNodesFromParent();
                //
                //     var product = _modelQuery.FindByNameAsync(model.Name);
                //     if (product is null)
                //         _modelQuery.CreateAsync(model);
                //     else
                //     {
                //         model.ID = product.ID;
                //         _modelQuery.UpdateAsync(model);
                //     }
                //
                //     LastItemScraped = model.Name;
                //     NumberOfItemsScraped++;
                //     
                // });
                // callBack?.Invoke();
            }

            IsDone = true;
            callBack?.Invoke();
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