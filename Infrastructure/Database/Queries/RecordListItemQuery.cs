using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.Record.Queries;
using Dapper;
using Inventory.Domain.Models;

namespace Infrastructure.Database.Queries
{
    public class RecordListItemQuery : QueryBase, IRecordListItemQuery
    {
        public RecordListItemQuery(IApplicationDbAccess dbAccess) : base(dbAccess)
        {
        }

        public async Task<RecordListItem> CreateAsync(RecordListItem model)
        {
            const string sql = @"INSERT INTO RecordItem (RecordID, ProductID, Quantity)
                            VALUES (@RecordID, @ProductID, @Quantity)";
            var prams = new
            {
                RecordID = model.RecordModel.ID,
                model.ProductID,
                model.Quantity
            };

            await _dbAccess.SaveDataAsync(sql, prams);
            return model;
        }

        public async Task UpdateAsync(RecordListItem model)
        {
            const string sql = @"UPDATE RecordItem 
                          SET Quantity = @Quantity 
                          WHERE ProductID = @ID;";
            var prams = new { model.Quantity, ID = model.ProductID };
            await _dbAccess.SaveDataAsync(sql, prams);
        }

        public async Task DeleteAsync(RecordListItem model)
        {
            const string sql = @"DELETE FROM RecordItem 
                            WHERE ProductID = @ID;";
            var prams = new { ID = model.ProductID };
            await _dbAccess.SaveDataAsync(sql, prams);
        }

        public async Task<IEnumerable<RecordListItem>> LoadAllAsync(RecordModel recordModel)
        {
            const string sql = @"SELECT P.ID, Name, Description, 
                                        UPC, Cost, Unit, URL, LastUpdated, ImageHref, Quantity 
                            FROM RecordItem 
                            JOIN Product P on P.ID = RecordItem.ProductID where @ID == RecordItem.RecordID;";

            var results = await _dbAccess
                .Connection
                .QueryAsync<ProductModel, int, Tuple<ProductModel, int>>(sql, Tuple.Create, recordModel);

            return results.Select(x => new RecordListItem(x.Item1, recordModel) { Quantity = x.Item2 });
        }
    }
}