using System.Collections.Generic;

namespace Inventory.Core
{
    public interface IRecordItemsQuery
    {
        public IEnumerable<ProductModel> LoadAll(RecordModel record);
        public void UpdateProduct(RecordModel record, ProductModel product);
        public void Delete(RecordModel record, ProductModel product);

    }
}