using System.Threading.Tasks;
using Application.Core.Common.Interfaces;
using Inventory.Domain.Models;

namespace Application.Core.Models.Product.Queries
{
    public interface IProductModelQuery : IQuery<ProductModel>
    {
        public Task<ProductModel> FindByNameAsync(string name);
    }
}