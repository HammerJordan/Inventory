using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Common.Interfaces;
using Application.Core.Models.Product.Queries;
using Inventory.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database.Queries
{
    public class ProductSearchQuery : QueryBase, IProductSearchQuery
    {
        public ProductSearchQuery(IApplicationDbAccess dbAccess) : base(dbAccess)
        {
        }

        public async Task<IEnumerable<ProductModel>> SearchAsync(string search, CancellationToken cancelToken)
        {
            var output = new List<ProductModel>();
            var columnsToSearch = ColumnsToSearchIn(search);

            if (columnsToSearch == null)
                return output;

            foreach (string column in columnsToSearch)
            {
                if (cancelToken.IsCancellationRequested)
                    return null;
                
                string query = @$"SELECT *
                                    FROM Product
                                    where {column} like ('%' || @search || '%');";
                var args = new { search };
                var results = await _dbAccess.LoadDataAsync<ProductModel,object>(query, args);
                
                output.AddRange(results.Where(x => output.All(y => y.ID != x.ID)));
            }

            return output;
        }
        
        private static IEnumerable<string> ColumnsToSearchIn(string search)
        {
            if (search.Length < 3)
                return null;

            return ContainsOnlyNumbers(search) ? 
                new[] { "UPC", "Name" } : 
                new[] { "Description", "Name" };
        }

        private static bool ContainsOnlyNumbers(string search)
        {
            var r = new Regex("[^\\d]");
            var match = r.Match(search);
            return !match.Success;
        }


    }
}