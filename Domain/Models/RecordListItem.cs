using Inventory.Domain.Enums;

namespace Inventory.Domain.Models
{
    public class RecordListItem
    {
        public ProductModel ProductModel { get; }
        public RecordModel RecordModel { get; }

        public int ID { get; set; }
        public int Quantity { get; set; } = 1;

        public int ProductID => ProductModel.ID;
        public string Name => ProductModel.Name;
        public string Description => ProductModel.Description;
        public string Upc => ProductModel.UPC;
        public decimal Cost => ProductModel.Cost;
        public string Unit => ProductModel.Unit;
        public string Url => ProductModel.URL;
        public string ImageHref => ProductModel.ImageHref;

        public RecordListItem(ProductModel productModel, RecordModel recordModel)
        {
            ProductModel = productModel;
            RecordModel = recordModel;
        }
    }
}