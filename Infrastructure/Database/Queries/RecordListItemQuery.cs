using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Interfaces;
using Application.Models.Record;
using Application.Models.Record.Queries;
using Dapper;
using Infrastructure.Exceptions;
using Inventory.Domain.Models;

namespace Infrastructure.Database.Queries
{
    public class RecordListItemQuery : QueryBase, IRecordListItemQuery
    {
        public RecordListItemQuery(IApplicationDbAccess dbAccess) : base(dbAccess)
        {
        }

        public RecordListItem Get(int id)
        {
            throw new NotImplementedException();
        }

        public RecordListItem Create(RecordListItem model)
        {
            const string sql = @"INSERT INTO RecordItem (RecordID, ProductID, Quantity)
                            VALUES (@RecordID, @ProductID, @Quantity)";
            var prams = new
            {
                RecordID = model.RecordModel.ID,
                model.ProductID,
                model.Quantity
            };

            _dbAccess.SaveData(sql, prams);
            return model;
        }

        public void Update(RecordListItem model)
        {
            const string sql = @"UPDATE RecordItem 
                          SET Quantity = @Quantity 
                          WHERE ProductID = @ID;";
            var prams = new { Quantity = model.Quantity, ID = model.ProductID };
            _dbAccess.SaveData(sql, prams);
        }

        public void Delete(RecordListItem model)
        {
            const string sql = @"DELETE FROM RecordItem 
                            WHERE ProductID = @ID;";
            var prams = new { ID = model.ProductID };
            _dbAccess.SaveData(sql, prams);
        }

        public IEnumerable<RecordListItem> LoadAll(RecordModel recordModel)
        {
            var sql = @"SELECT P.ID, Name, Description, UPC, Cost, Unit, URL, LastUpdated, ImageHref, Quantity 
                            FROM RecordItem 
                            JOIN Product P on P.ID = RecordItem.ProductID where @ID == RecordItem.RecordID;";

            var results = _dbAccess
                .Connection
                .Query<ProductModel, int, Tuple<ProductModel, int>>(sql, Tuple.Create, recordModel);

            return results.Select(x => new RecordListItem(x.Item1, recordModel) { Quantity = x.Item2 });
        }
    }
}