using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Application.Common.Interfaces;
using Application.Models.Record.Queries;
using Infrastructure.Exceptions;
using Inventory.Domain.Models;
using log4net;

namespace Infrastructure.Database.Queries
{
    public class RecordModelQuery : QueryBase, IRecordModelQuery
    {
        public RecordModelQuery(IApplicationDbAccess dbAccess) : base(dbAccess)
        {
        }

        public RecordModel Get(int id)
        {
            string sql = "SELECT * FROM Record WHERE ID == @id LIMIT 1;";
            var result = _dbAccess.LoadData<RecordModel,int>(sql, id).FirstOrDefault();
            
            if (result == null)
                throw new NotFoundInDbException($"No row found with an ID:{id}");

            return result;

        }

        public RecordModel Create(RecordModel model)
        {
            var sql = @"INSERT INTO Record (Name, CreatedDateTime) 
                            VALUES ('@Name',@CreatedDate)";
            
            var prams = new
            {
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };
            _dbAccess.SaveData(sql, prams);
            
            sql = @"SELECT *
                        FROM Record
                        WHERE ID = (SELECT MAX(ID)  FROM Record);";
            
            return _dbAccess.LoadData<RecordModel, dynamic>(sql, null).First();
        }

        public void Update(RecordModel model)
        {
            const string sql = @"UPDATE Record
                            Set Name = @Name
                            Where ID = @ID;";
            _dbAccess.SaveData(sql, model);
        }

        public void Delete(RecordModel model)
        {
            const string sql = @"DELETE from Record where ID = @ID;";
            _dbAccess.SaveData(sql, model);
        }

        public IEnumerable<RecordModel> LoadAll()
        {
            const string sql = "SELECT * FROM Record";

            return _dbAccess.LoadData<RecordModel, dynamic>(sql, null);
        }
    }
}