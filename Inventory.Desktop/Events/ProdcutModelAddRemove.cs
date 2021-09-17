using InventoryManagement.Core.Models;

namespace InventoryManagement.Desktop.Events
{
    public class ProductModelAddRemove
    {
        public readonly ProductModel Model;

        public ProductModelAddRemove(ProductModel model)
        {
            this.Model = model;
        }
    }
}