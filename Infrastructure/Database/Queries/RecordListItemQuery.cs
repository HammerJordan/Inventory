using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Common.Interfaces;
using Application.Core.Models.Record.Queries;
using Application.Core.Models.RecordProductList.Queries;
using Dapper;
using Inventory.Domain.Models;
using Microsoft.Extensions.Logging;

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
            const string sql = @"SELECT Quantity, P.ID AS ID, Name, Description, 
                                        UPC, Cost, Unit, URL, LastUpdated, ImageHref
                            FROM RecordItem 
                            JOIN Product P on P.ID = RecordItem.ProductID where @ID == RecordItem.RecordID;";

            // var res = await _dbAccess.Connection.QueryAsync<ProductModel, int, RecordListItem>(
            //     sql, ((model, i) => new RecordListItem(model, recordModel) { Quantity = i }), recordModel,
            //     splitOn: "ID");

            var results = await _dbAccess.Connection.QueryAsync<long, ProductModel, RecordListItem>(
                sql, (quantity, product) =>
                    new RecordListItem(product, recordModel)
                    {
                        Quantity = (int)quantity
                    }, recordModel, splitOn: "ID");

            return results;
        }
    }
}