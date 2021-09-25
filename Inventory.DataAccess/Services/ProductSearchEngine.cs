using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Inventory.Core;

namespace Inventory.DataAccess
{
    public class ProductSearchEngine
    {
        private const string DESCRIPTION = "Description";
        private const string NAME = "Name";
        private const string UPC = "UPC";
        
        private readonly ISqlLiteDataAccess dataAccess;
        

        public ProductSearchEngine(ISqlLiteDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<ProductModel> SearchResults(string search)
        {
            var columnsToSearch = ColumnsToSearchIn(search);

            if (columnsToSearch == null)
                return null;

            var output = new List<ProductModel>();

            foreach (string column in columnsToSearch)
            {
                string query = @$"
                    SELECT *
                    FROM Product
                    where {column} like ('%' || @search || '%');";


                var results = dataAccess.LoadData<ProductModel,object>(query, new {search});

                output.AddRange(results.Where(x => output.All(y => y.ID != x.ID)));

                //foreach (var result in results.Where(result => output.All(x => x.ID != result.ID)))
                //{
                //    //TODO bring the images in later
                //    var image = dataAccess.LoadData<byte[], object>(imageQuery, new { Id = result.ID });
                //    if (image != null && image.Count > 0)
                //    {
                //        //TODO: add images back
                //        //  using var toImage = new MemoryStream(image.First());
                //        // MediaTypeNames.Image imageFromBytes = MediaTypeNames.Image.FromStream(toImage);
                //        // result.Image = imageFromBytes;
                //    }


                //    output.Add(result);
                //}

            }

            return output.Count == 0 ? null : output;

        }

        public IEnumerable<string> ColumnsToSearchIn(string search)
        {
            if (search.Length < 3)
                return null;
            
            if(ContainsOnlyNumbers(search))
                return new[] {UPC, NAME};
            
            return new[] {DESCRIPTION, NAME};
        }

        private static bool ContainsNumbers(string search)
        {
            var r = new Regex("\\d");
            var match = r.Match(search);
            return match.Success;
        }

        private static bool ContainsOnlyNumbers(string search)
        {
            var r = new Regex("[^\\d]");
            var match = r.Match(search);
            return !match.Success;
        }
        
        
    }
}