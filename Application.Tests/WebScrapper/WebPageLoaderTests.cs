using AngleSharp.Html.Dom;
using Application.WPF.WebScraping;
using Xunit;
using FluentAssertions;

namespace WebScraping.Test
{
    public class WebPageLoaderTests
    {
        private readonly WebPageLoader pageLoader = new WebPageLoader();

        [Fact]
        public async void GetWebPage_ReturnValidIHtmlDoc_WhenUrlIsValid()
        {
            string url =
                @"https://ebhorsman.com/itemDetail?product=6641-w-energy-management-toggle-dimmer-leviton&p=12984";

            IHtmlDocument page = await pageLoader.GetWebPageAsync(url);

            page.Should().NotBeNull();
        }
        

    }
}