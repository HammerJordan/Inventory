using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Inventory.DataAccess;
using Inventory.Core;
using Microsoft.Extensions.Configuration;
using WebScraping;

namespace Inventory.Desktop.Services
{
    public class DatabaseUpdate
    {
        private readonly ISqlLiteDataAccess dataAccess;
        private readonly IConfiguration configSettings;
        private readonly ProductScraper webProductScraper;

        public DatabaseUpdate(ISqlLiteDataAccess dataAccess,
            IConfiguration configSettings,
            ProductScraper webProductScraper)
        {
            this.dataAccess = dataAccess;
            this.configSettings = configSettings;
            this.webProductScraper = webProductScraper;
        }

        public async Task RunUpdateDatabaseAsync()
        {
            var ignoreCategories = configSettings
                .GetSection("IgnoreProducts")
                .GetChildren()
                .Select(x => x.Value)
                .ToArray();

            var entryUrl = configSettings["EntryUrl"];

            await Task.Run(() => UpdateDatabaseRunner(ignoreCategories, entryUrl));
        }

        private async Task UpdateDatabaseRunner(string[] ignore, string url)
        {
            //120Mins
            var results = await webProductScraper
                .GetAllModelsFromCategoriesAsync(url, ignore);

            var query = @"  select *
                            from Product
                            where Name = @name";

            foreach (var productModel in results)
            {
                var result = dataAccess
                    .LoadData<ProductModel, object>
                        (query, new { name = productModel.Name });

                if (result == null || result.Count == 0)
                {
                    var sql = @$"INSERT INTO Product (Name, Description, UPC, Cost, Unit, URL, LastUpdated)
                              VALUES (@Name, @Description, @UPC, @Cost, @Unit, @URL, @NowToString)";
                    dataAccess.SaveData(sql, productModel);
                }
                else
                {
                    var sql = @"
                        Update Product
                        set
                            Description = @Description,
                            UPC = @UPC,
                            Cost = @Cost,
                            URL = @URL,
                            LastUpdated = @NowToString
                        where ID = @ID";

                    dataAccess.SaveData(sql, productModel);
                }

                result = dataAccess
                    .LoadData<ProductModel, object>
                        (query, new { name = productModel.Name });

                int id = result.First().ID;

                var imgSql = @"select *
                            from Product_Image
                            where ProductID = @id";

                // var count = dataAccess.LoadData<int, object>(imgSql, new { id = id });
                //
                // if (count.Count != 0)
                //     continue;
                //
                // if (productModel.Image == null)
                //     continue;
                //
                // var sqlInsertImage = @"INSERT INTO Product_Image 
                //                            (ProductID, Image)
                //                             VALUES(@ProductID, @Image)";
                // var imageByte = Image2Byte(productModel.Image);
                //
                // dataAccess.SaveData(sqlInsertImage, new { ProductID = id, Image = imageByte });
            }
        }

        // private async Task Test()
        // {
        //     WebPageLoader pageLoader = new WebPageLoader();
        //
        //     string url =
        //         @"https://ebhorsman.com/itemDetail?p=284100";
        //     IHtmlDocument page = await pageLoader.GetWebPageAsync(url);
        //     //var Model = webProductScraper.GetProductModel(page);
        //
        //     var query = @"  select *
        //                     from Product
        //                     where Name = @name";
        //
        //     var result = dataAccess
        //         .LoadData<ProductModel, object>
        //             (query, new { name = Model.Name });
        //
        //     int id = result.First().ID;
        //
        //     var imgSql = @"select *
        //                     from Product_Image
        //                     where ProductID = @id";
        //
        //     var count = dataAccess.LoadData<int, object>(imgSql, new { id = id });
        //
        //     if (count.Count != 0)
        //         return;
        //
        //     var sqlInsertImage = @"INSERT INTO Product_Image 
        //                                    (ProductID, Image)
        //                                     VALUES(@ProductID, @Image)";
        //     var imageByte = Image2Byte(Model.Image);
        //
        //     dataAccess.SaveData(sqlInsertImage, new { ProductID = id, Image = imageByte });
        // }
        //
        // private byte[] Image2Byte(Image imageToconvert)
        // {
        //     using var memoryStream = new MemoryStream();
        //     var bpm = new Bitmap(imageToconvert);
        //
        //     bpm.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
        //     byte[] bytesOfImage = memoryStream.ToArray();
        //     return bytesOfImage;
        // }
        //
        // private Image ByteToImage(byte[] toConvert)
        // {
        //     MemoryStream toImage = new MemoryStream(toConvert);
        //     Image imageFromBytes = Image.FromStream(toImage);
        //     return imageFromBytes;
        // }
    }
}