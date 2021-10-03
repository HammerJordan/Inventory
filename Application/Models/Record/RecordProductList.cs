using System.Collections.Generic;
using System.Linq;
using Inventory.Domain.Models;

namespace Application.Models.Record
{
    public class RecordProductList
    {
        private readonly List<RecordListItem> _products = new();
        public RecordModel Record { get; }

        public IReadOnlyList<RecordListItem> ProductLists => _products;

        public RecordProductList(RecordModel record)
        {
            Record = record;
        }

        public void Add(ProductModel productModel, int amount = 1)
        {
            if (_products.Any(x => x.ID == productModel.ID))
                _products.First(x => x.ID == productModel.ID).Quantity += amount;
            else
                _products.Add(new RecordListItem(productModel, Record) { Quantity = amount });
        }

        public void Subtract(ProductModel productModel, int amount = 1)
        {
            if (_products.All(x => x.ID != productModel.ID))
                return;

            var productItem = _products.First(x => x.ID == productModel.ID);
            productItem.Quantity -= amount;
            if (productItem.Quantity < 0)
                productItem.Quantity = 0;
        }

        public void Remove(ProductModel productModel)
        {
            if (_products.All(x => x.ID != productModel.ID))
                return;
            var productItem = _products.First(x => x.ID == productModel.ID);
            _products.Remove(productItem);
        }
    }
}