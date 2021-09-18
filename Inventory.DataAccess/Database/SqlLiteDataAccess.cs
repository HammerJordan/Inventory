using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Inventory.Core;

namespace Inventory.DataAccess
{
    public class SqlLiteDataAccess : IDisposable, ISqlLiteDataAccess
    {
        private IDbConnection dbConnection;
        private IDbTransaction dbTransaction;
        private bool isClosed;


        public SqlLiteDataAccess(DbConnection connection)
        {
            dbConnection = connection.GetDBConnection();
        }

        public List<T> LoadData<T, TPrams>(string storedProcedure, TPrams parameters)
        {
            var rows = dbConnection.Query<T>(storedProcedure, parameters).ToList();

            return rows;
        }

        public void SaveData<T>(string storedProcedure, T parameters)
        {
            dbConnection.Execute(storedProcedure, parameters);
        }


        public void StartTransaction()
        {
            dbConnection.Open();

            dbTransaction = dbConnection.BeginTransaction();
            isClosed = false;
        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            dbConnection.Execute(storedProcedure,
                                 parameters,
                                 dbTransaction);

        }

        public List<T> LoadDataInTransaction<T, TPrams>(string storedProcedure, TPrams parameters)
        {
            return dbConnection.Query<T>(storedProcedure,
                                             parameters,
                                             dbTransaction).ToList();
        }

        public void CommitTransaction()
        {
            dbTransaction?.Commit();
            dbConnection?.Close();

            isClosed = true;
        }

        public void RollbackTransaction()
        {
            dbTransaction?.Rollback();
            dbConnection?.Close();
            isClosed = true;
        }


        public void Dispose()
        {
            if (isClosed == false)
            {
                CommitTransaction();
            }

            dbTransaction = null;
            dbConnection = null;
        }
    }
}