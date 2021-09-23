using Inventory.Core;

namespace Inventory.Desktop.Events
{
    public class ProductModelAddRemove
    {
        public readonly ProductModel Model;

        public ProductModelAddRemove(ProductModel model)
        {
            Model = model;
        }
    }
}