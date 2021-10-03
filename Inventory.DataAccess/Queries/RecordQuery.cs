using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Inventory.Domain;

namespace Inventory.DataAccess.Queries
{
    public class RecordQuery : IRecordQuery
    {
        private readonly ISqlLiteDataAccess dataAccess;

        public RecordQuery(ISqlLiteDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<RecordModel> LoadAll()
        {
            var sql = "Select * from Record";

            return dataAccess.LoadData<RecordModel, dynamic>(sql, null);
        }

        public RecordModel Create()
        {
            var sql = $"INSERT INTO Record (Name, CreatedDateTime)" +
                      $"VALUES ('',@CreatedDate)";
            var prams = new { CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture) };

            dataAccess.SaveData(sql, prams);

            sql = @"SELECT * 
                        FROM Record
                        WHERE ID = (SELECT MAX(ID)  FROM Record); ";

            return dataAccess.LoadData<RecordModel, dynamic>(sql, null).First();
        }

        public void Update(RecordModel record)
        {
            var sql = @"UPDATE Record
                            Set Name = @Name
                            Where ID = @ID;";
            dataAccess.SaveData(sql, record);
        }

        public void Delete(RecordModel record)
        {
            var sql = @"DELETE from Record where ID = @ID;";
            dataAccess.SaveData(sql, record);
        }
    }
}