using System.Linq;
using AngleSharp.Html.Dom;
using Application.WPF.WebScraping;
using FluentAssertions;
using Xunit;

namespace WebScraping.Test
{
    public class ProductScraperTests
    {
        private readonly WebPageLoader pageLoader;
        private readonly ProductScraper productScraper;

        public ProductScraperTests()
        {
            pageLoader = new WebPageLoader();
            productScraper = new ProductScraper(pageLoader);
        }
        
        [Fact]
        public async void GetProductsElementsFromProductListView()
        {
            string url = @"https://ebhorsman.com/category-items?type=dimming-colour-change-kits&cat=340501";
            IHtmlDocument page = await pageLoader.GetWebPageAsync(url);
            var results = productScraper.GetProductLinksFromPage(page);
            results.Count.Should().Be(20);
        }

        [Fact]
        public async void GetProductModelFromIElement()
        {
            string url = @"https://ebhorsman.com/category-items?type=dimming-colour-change-kits&cat=340501";
            IHtmlDocument page = await pageLoader.GetWebPageAsync(url);
            var result = productScraper.GetProductLinksFromPage(page).First();

            var model = productScraper.GetProductModel(result);
            model.Name.Should().BeEquivalentTo("IPKIT-E");
            model.Description
                .Should().ContainEquivalentOf("Leviton® Color Change Kits for IllumaTech Dimmer - black");
            model.UPC.Should().BeEquivalentTo("07847701044");
            model.Unit.Should().BeEquivalentTo("each");
            model.Cost.Should().Be(5.52m);

            model.URL.Should()
                .BeEquivalentTo(
                    "itemDetail?product=ipkit-e-energy-management-colour-change-kit-&p=49241");

            model.ImageHref.Should()
                .BeEquivalentTo("https://storage.googleapis.com/ebh/images/product/49241.jpg");

        }
        
        
        [Fact]
        public async void GetProductModelFromIElement_WithUnitAs_PackageOf100()
        {
            string url = @"https://ebhorsman.com/searchresults?find=NMC050TB";
            IHtmlDocument page = await pageLoader.GetWebPageAsync(url);
            var result = productScraper.GetProductLinksFromPage(page).First();

            var model = productScraper.GetProductModel(result);
            model.Name.Should().BeEquivalentTo("NMC050TB");
            model.Description.Should()
                .ContainEquivalentOf("T&amp;B Industrial Fitting® Flexible Non-Metallic Conduit  1/2 Inch  X 100 Foot  Black");
            model.UPC.Should().BeEquivalentTo("78621010389");
            model.Unit.Should().BeEquivalentTo("package of 100");
            model.Cost.Should().Be(167.70m);

            model.URL.Should()
                .BeEquivalentTo(
                    "itemDetail?product=nmc050tb-t&b-flexible-non-metallic-conduit-&p=27230");

            model.ImageHref.Should()
                .BeEquivalentTo("https://storage.googleapis.com/ebh/images/product/27230.jpg");

        }        


        [Fact]
        public async void GetCategoriesLinks_ReturnAllCategories()
        {
            string url = @"https://ebhorsman.com/categories";
            IHtmlDocument page = await pageLoader.GetWebPageAsync(url);

            var results = productScraper.GetCategoriesLinks(page);

            results.Count.Should().Be(26);
        }

        [Fact]
        public async void GetGroupLinks_ReturnAllCategories()
        {
            string url = @"https://ebhorsman.com/category-groups?type=energy-management&cat=3400";
            IHtmlDocument page = await pageLoader.GetWebPageAsync(url);

            var results = productScraper.GetCategoriesLinks(page);

            results.Count.Should().Be(9);
        }

        [Fact]
        public async void GetProductLinks_ReturnsAllProductLinksOnAPage()
        {
            string url = @"https://ebhorsman.com/category-items?type=dimming-colour-change-kits&cat=340501";
            IHtmlDocument page = await pageLoader.GetWebPageAsync(url);

            var results = productScraper.GetProductLinksFromPage(page);

            results.Count.Should().Be(20);
        }

        [Fact]
        public async void GetNextProductLinkPage_ReturnsNextPageLinkOrNullIfNoNextPage()
        {
            string url = @"https://ebhorsman.com/category-items?type=dimming-colour-change-kits&cat=340501";
            IHtmlDocument page = await pageLoader.GetWebPageAsync(url);

            var result = productScraper.GetNextProductLinkPage(page);

            result.Should().BeEquivalentTo("category_items_test?cat=340501&page=2");
        }

        [Fact(Skip = "Slow only for debugging")]
        public async void GetAllModelsFromCategories_ShouldReturnModels()
        {
            string[] ignoreOptions = new[]
            {
                "Automation",
                "Ballasts",
                "Chemicals",
                "Clean Energy",
                "Conduit & Bends",
                "Controls",
                "Datacom Networking",
                "Distribution",
                "Enclosures",
                "Energy Management",
                "Fasteners",
                "Fuses",
                "Heating & Cooling",
                "Instrumentation",
                "Lamps",
                "Lighting",
                "RoughIn Fittings Boxes",
                "Safety Product",
                "Signaling",
                "Special Offers",
                "Tools",
                "Ventilation",
                "Wire & Cable",
                "Wire Management",
                "Wiring Devices"
            };

            string url = @"https://ebhorsman.com/categories";

            var result = await productScraper.GetAllModelsFromCategoriesAsync(url, ignoreOptions);

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async void GetGetProductModelFromUrl()
        {
            string url =
                "https://ebhorsman.com/itemDetail?product=ipkit-e-energy-management-colour-change-kit-&p=49241";

           var product = await productScraper.GetProductModelFromUrlAsync(url);

           product.Name.Should().BeEquivalentTo("IPKIT-E");
           product.Description.Should().BeEquivalentTo("Leviton® Color Change Kits for IllumaTech Dimmer - black");
           product.Cost.Should().Be(5.52m);
           product.Unit.Should().BeEquivalentTo("each");
           product.UPC.Should().BeEquivalentTo("07847701044");

        }
        

    }
}