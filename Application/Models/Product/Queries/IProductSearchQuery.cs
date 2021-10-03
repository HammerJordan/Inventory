using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Domain.Models;

namespace Application.Models.Product.Queries
{
    public interface IProductSearchQuery
    {
        public Task<IEnumerable<ProductModel>> SearchAsync(string search, CancellationToken cancelToken);
    }
}