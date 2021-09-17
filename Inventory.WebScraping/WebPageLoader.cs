using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace WebScraping
{
    public class WebPageLoader : IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly WebClient webClient;

        public WebPageLoader()
        {
            httpClient = new HttpClient();
            webClient = new WebClient();
        }

        public async Task<IHtmlDocument> GetWebPageAsync(string url)
        {
            var response = await httpClient.GetAsync(url);
            await using var content = await response.Content.ReadAsStreamAsync();
            var parser = new HtmlParser();
            var document = parser.ParseDocument(content);
            return document;
        }

        public IHtmlDocument GetWebPage(string url)
        {
            var response = httpClient.GetAsync(url);
            response.Wait();

            using var content = response.Result.Content.ReadAsStreamAsync();
            content.Wait();

            var parser = new HtmlParser();
            var document = parser.ParseDocument(content.Result);
            return document;
        }

        public Image GetImageFromUrl(string url)
        {
            using var memStream = new MemoryStream(webClient.DownloadData(url));
            return Image.FromStream(memStream);
        }

        private void ReleaseUnmanagedResources()
        {
            httpClient.Dispose();
            webClient.Dispose();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~WebPageLoader()
        {
            ReleaseUnmanagedResources();
        }
    }
}