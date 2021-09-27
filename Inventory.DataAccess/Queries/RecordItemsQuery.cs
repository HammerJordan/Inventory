using System.Collections.Generic;
using Inventory.Core;

namespace Inventory.DataAccess.Queries
{
    public class RecordItemsQuery : IRecordItemsQuery
    {
        private readonly ISqlLiteDataAccess dataAccess;

        public RecordItemsQuery(ISqlLiteDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<ProductModel> LoadAll(RecordModel record)
        {
            var sql = "select P.ID, Name, Description, UPC, Cost, Unit, URL, LastUpdated, ImageHref, Quantity " +
                      "from RecordItem " +
                      "join Product P on P.ID = RecordItem.ProductID where @ID == RecordItem.RecordID;";

            var prams = new { record.ID };
            var result = dataAccess.LoadData<ProductModel,object>(sql, prams);

            return result;
        }

        public void InsertProduct(RecordModel record, ProductModel product)
        {
            var sql = $"INSERT INTO RecordItem (RecordID, ProductID, Quantity)" +
                      $"VALUES (@RecordID, @ProductID, @Quantity)";
            var prams = new { RecordID = record.ID, ProductID = product.ID, Quantity = product.Quantity };

            dataAccess.SaveData(sql, prams);
        }

        public void UpdateProduct(RecordModel record, ProductModel product)
        {
            var sql = $"UPDATE RecordItem " +
                      $"set Quantity = @Quantity " +
                      $"where ProductID = @ID;";
            var prams = new { Quantity = product.Quantity, ID = product.ID };
            dataAccess.SaveData(sql, prams);
        }


        public void Delete(RecordModel record, ProductModel product)
        {
        }
    }
}