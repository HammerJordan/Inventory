using System.Threading.Tasks;
using Inventory.Domain.Models;

namespace Application.WPF.WebScraping.Common
{
    public interface ICheckProductForUpdates
    {
        public Task<ProductModel> CheckForUpdatesAsync(ProductModel model);
    }
}