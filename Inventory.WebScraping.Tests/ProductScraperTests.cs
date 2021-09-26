﻿using System.Linq;
using AngleSharp.Html.Dom;
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
        
        


        //[Fact]
        //public async void GetProductModel_ReturnValidProductModel()
        //{
        //    string url =
        //        @"https://ebhorsman.com/itemDetail?product=6641-w-energy-management-toggle-dimmer-leviton&p=12984";
        //    IHtmlDocument page = await pageLoader.GetWebPageAsync(url);

        //    var model = productScraper.GetProductModel(page);

        //    model.Name.Should().BeEquivalentTo("6641-W");

        //    model.Description.Should()
        //        .BeEquivalentTo(@"Leviton® Electro-Mechanical Dimmer, 120 VAC, 1 Pole, Standard On/Off Mode, white");

        //    model.UPC.Should().BeEquivalentTo("06867919053");

        //    model.Cost.Should().Be(25.25);

        //    model.Unit.Should().BeEquivalentTo("each");
        //}

        //[Fact]
        //public async void GetCategoriesLinks_ReturnAllCategories()
        //{
        //    string url = @"https://ebhorsman.com/categories";
        //    IHtmlDocument page = await pageLoader.GetWebPageAsync(url);

        //    var results = productScraper.GetCategoriesLinks(page);

        //    results.Count.Should().Be(26);
        //}

        //[Fact]
        //public async void GetGroupLinks_ReturnAllCategories()
        //{
        //    string url = @"https://ebhorsman.com/category-groups?type=energy-management&cat=3400";
        //    IHtmlDocument page = await pageLoader.GetWebPageAsync(url);

        //    var results = productScraper.GetCategoriesLinks(page);

        //    results.Count.Should().Be(9);
        //}

        //[Fact]
        //public async void GetProductLinks_ReturnsAllProductLinksOnAPage()
        //{
        //    string url = @"https://ebhorsman.com/category-items?type=dimming-colour-change-kits&cat=340501";
        //    IHtmlDocument page = await pageLoader.GetWebPageAsync(url);

        //    var results = productScraper.GetProductLinksFromPage(page);

        //    results.Count.Should().Be(30);
        //}

        //[Fact]
        //public async void GetNextProductLinkPage_ReturnsNextPageLinkOrNullIfNoNextPage()
        //{
        //    string url = @"https://ebhorsman.com/category-items?type=dimming-colour-change-kits&cat=340501";
        //    IHtmlDocument page = await pageLoader.GetWebPageAsync(url);

        //    var result = productScraper.GetNextProductLinkPage(page);

        //    result.Should().BeEquivalentTo("category-items?cat=340501&page=2");
        //}

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
    }
}