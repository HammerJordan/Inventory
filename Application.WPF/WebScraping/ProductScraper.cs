using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Inventory.Domain.Models;
// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace Application.WPF.WebScraping
{
    public class ProductScraper
    {
        private readonly WebPageLoader pageLoader;

        public ProductScraper(WebPageLoader pageLoader)
        {
            this.pageLoader = pageLoader;
        }

        public ProductModel GetProductModel(IElement productElement)
        {
            var product = new ProductModel
            {
                Name = GetName(productElement),
                Description = GetDescription(productElement),
                UPC = GetUpc(productElement),
                Cost = GetCost(productElement),
                Unit = GetUnit(productElement),
                URL = GetUrl(productElement),
                ImageHref = GetImageHref(productElement)
            };
            return product;
        }

        public async Task<ProductModel> GetProductModelFromUrlAsync(string url)
        {
            var page = await pageLoader.GetWebPageAsync(url);

            var product = new ProductModel
            {
                Name = GetName(page),
                Description = GetDescription(page),
                UPC = GetUpc(page),
                Cost = GetCost(page),
                Unit = GetUnit(page),
                URL = url,
                ImageHref = GetImageHref(page)
            };
            return product;
        }

        public List<DirectoryNode> GetCategoriesLinks(IHtmlDocument page, params string[] exclude)
        {
            var output = new List<DirectoryNode>();

            var parent = page.All.FirstOrDefault(x => x.ClassName == "row category-child");

            if (parent == null)
                return null;

            foreach (var child in parent.Children)
            {
                string href = child.Children[0].GetAttribute("href");
                string name = child.Children[0].Children[1].Children[0].Children[0].TextContent;

                if (exclude.Any(x => string.Equals(x, name, StringComparison.CurrentCultureIgnoreCase)))
                    continue;

                output.Add(new DirectoryNode { Category = name, Url = href });
            }

            return output;
        }

        public List<IElement> GetProductLinksFromPage(IHtmlDocument page)
        {
            return page
                .All
                .Where(x => x.ClassName == "search-result row")
                .ToList();
        }

        public string GetNextProductLinkPage(IHtmlDocument page)
        {
            var result = page.QuerySelector("nav[aria-label='Page navigation']")?.Children[0];

            if (result is null || result.Children.Length <= 1)
                return null;

            string link = result.Children[1].Children[0].GetAttribute("href");

            if (link == "#")
                return null;

            return link.Replace("../", "");
        }

        public async Task<List<ProductModel>> GetAllModelsFromCategoriesAsync(string entryUrl,
            params string[] exclude)
        {
            var directoryTree = new DirectoryTreeModel { RootURL = entryUrl };

            var tasks = new List<Task>();

            var page = await pageLoader.GetWebPageAsync(entryUrl);
            var categoryLinks = GetCategoriesLinks(page, exclude);

            directoryTree.Nodes.AddRange(categoryLinks);

            foreach (var categoryLink in directoryTree.Nodes)
                tasks.Add(Task.Run(async () => await WalkCategoryGroups(categoryLink, directoryTree)));

            await Task.WhenAll(tasks);

            tasks.Clear();

            var leafNodes = directoryTree.GetLeafNodes();

            var products = new List<ProductModel>();

            foreach (var node in leafNodes)
            {
                page = await pageLoader.GetWebPageAsync(directoryTree.GetUrlFromHref(node.Url));
                var productLinks = new List<string>();

                while (true)
                {
                    // productLinks.AddRange(GetProductLinksFromPage(page));
                    string next = GetNextProductLinkPage(page);
                    if (next is null)
                        break;
                    page = await pageLoader.GetWebPageAsync(directoryTree.GetUrlFromHref(next));
                }

                foreach (string productLink in productLinks)
                {
                    //page = await pageLoader.GetWebPageAsync(directoryTree.GetUrlFromHref(productLink));
                    //var model = GetProductModel(page);
                    //model.URL = directoryTree.GetUrlFromHref(productLink);

                    //var parent = node.GetRootNode();
                    //var groups = node.GetBranchNodesFromParent();

                    //model.Category = parent.Category;
                    //model.Group = groups
                    //    .Select(x => x.Category)
                    //    .Aggregate((c, n) => c + "\\" + n);

                    //products.Add(model);
                }
            }

            return products;
        }

        public async Task WalkCategoryGroups(DirectoryNode parent,
            DirectoryTreeModel directoryTree,
            params string[] exclude)
        {
            string url = directoryTree.GetUrlFromHref(parent.Url);
            var page = await pageLoader.GetWebPageAsync(url);

            var categoryLinks = GetCategoriesLinks(page, exclude);
            if (categoryLinks is null)
                return;

            parent.Children.AddRange(categoryLinks);

            foreach (var child in parent.Children)
                await WalkCategoryGroups(child, directoryTree, exclude);
        }

        private static string GetName(IElement productElement)
        {
            return productElement.QuerySelectorAll("a").First(x => x.ClassName == "mobile-no").InnerHtml;
        }

        private static string GetName(IHtmlDocument productElement)
        {
            string result = productElement.QuerySelectorAll(".product-title")
                .FirstOrDefault()?
                .Children?.FirstOrDefault()?.InnerHtml;
            return result;
        }

        private static string GetDescription(IElement productElement)
        {
            return productElement.QuerySelectorAll("p").First(x => x.ClassName == "mobile-no").InnerHtml;
        }

        private static string GetDescription(IHtmlDocument productElement)
        {
            string result = productElement
                .QuerySelectorAll(".product-desc").FirstOrDefault()
                ?.InnerHtml;

            return result;
        }

        private static string GetUpc(IElement productElement)
        {
            var query = productElement.QuerySelectorAll("div");
            var upcDiv = query.FirstOrDefault(x => x.InnerHtml.Contains("UPC:"));
            if (upcDiv == null)
                return "";
            string upcInnerHtml = upcDiv.InnerHtml;
            int start = upcInnerHtml.IndexOf("UPC: </span>", StringComparison.Ordinal) + 12;
            upcInnerHtml = upcInnerHtml[start..];
            int end = upcInnerHtml.IndexOf("</", StringComparison.Ordinal);

            string result = upcInnerHtml[..end];

            return result;
        }

        private static string GetUpc(IHtmlDocument productElement)
        {
            var query = productElement.QuerySelectorAll("div");
            var upcDiv = query.FirstOrDefault(x => x.InnerHtml.Contains("UPC:"));
            if (upcDiv == null)
                return "";
            string upcInnerHtml = upcDiv.InnerHtml;
            int start = upcInnerHtml.IndexOf("UPC: </span>", StringComparison.Ordinal) + 12;
            upcInnerHtml = upcInnerHtml[start..];
            int end = upcInnerHtml.IndexOf("</", StringComparison.Ordinal);

            string result = upcInnerHtml[..end];

            return result;
        }

        private static string GetUnit(IElement productElement)
        {
            string inner = productElement.QuerySelectorAll("li")[1].InnerHtml;
            int end = inner.IndexOf(';') + 1;
            return inner[end..];
        }

        private static string GetUnit(IHtmlDocument productElement)
        {
            var inner = productElement.QuerySelectorAll(".product-stock").FirstOrDefault();
            if (inner == null)
                return "";
            
            string innerHtml = inner.InnerHtml;
            int start = inner.InnerHtml.IndexOf(";") + 1;
            innerHtml = innerHtml[start..];
            int end = innerHtml.IndexOf(" ");

            return innerHtml[..end];
        }

        private static decimal GetCost(IElement productElement)
        {
            string inner = productElement.QuerySelectorAll("li")[0].InnerHtml;
            int end = inner.IndexOf(';') + 1;
            return decimal.Parse(inner[end..]);
        }

        private static decimal GetCost(IHtmlDocument productElement)
        {
            var innerElement = productElement.QuerySelector(".product-stock");
            var unOrderedListElement = innerElement.QuerySelector("ul");
            string innerHtml = unOrderedListElement.InnerHtml;
            int start = innerHtml.IndexOf("</i>") + 4;

            innerHtml = innerHtml[start..];
            int end = innerHtml.IndexOf(" ");
            innerHtml = innerHtml[..end];

            return decimal.Parse(innerHtml);
        }

        private static string GetUrl(IElement productElement)
        {
            return productElement
                .QuerySelectorAll("a")
                .First(x => x.ClassName == "mobile-no")
                .GetAttribute("href");
        }

        private static string GetImageHref(IElement productElement)
        {
            return productElement
                .QuerySelector("a.thumbnail")
                .GetElementsByTagName("img")
                .First()
                .GetAttribute("src");
        }

        private static string GetImageHref(IHtmlDocument productElement)
        {
            var result = productElement.QuerySelector("#item-display");
            return result.GetAttribute("src");
        }
    }
}