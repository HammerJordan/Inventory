using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Core;

namespace Inventory.DataAccess
{
    public class InvoiceDBHelper
    {
        private readonly ISqlLiteDataAccess dataAccess;


        public InvoiceDBHelper(ISqlLiteDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<RecordModel> LoadInvoices()
        {
            var sql = "Select * from Record";

            return dataAccess.LoadData<RecordModel,dynamic>(sql, null);
        }

        public  RecordModel CreateNewInvoice()
        {

            var sql = $"INSERT INTO Record (Name, CreatedDateTime)" +
                      $"VALUES ('',@CreatedDate)";
            var prams = new { CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture) };

            dataAccess.SaveData(sql,prams);

            sql = @"SELECT * 
                        FROM Record
                        WHERE ID = (SELECT MAX(ID)  FROM Record); ";

            return dataAccess.LoadData<RecordModel,dynamic>(sql,null).First();
        }

        public void SaveRecordModel(RecordModel model)
        {
            var sql = @"UPDATE Record
                            Set Name = @Name
                            Where ID = @ID;";
            dataAccess.SaveData(sql,model);
        }

        public void DeleteRecordModel(RecordModel model)
        {
            var sql = @"DELETE from Record where ID = @ID;";
            dataAccess.SaveData(sql, model);
        }

        //TODO: GetInvoice Items


    }
}