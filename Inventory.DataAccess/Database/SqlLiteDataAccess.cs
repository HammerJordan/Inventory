using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;

namespace Inventory.DataAccess
{
    public class SqlLiteDataAccess : IDisposable, ISqlLiteDataAccess
    {
        private IDbConnection dbConnection;
        private IDbTransaction dbTransaction;
        private bool isClosed = false;


        public SqlLiteDataAccess()
        {
        }

        public string GetConnectionString()
        {
            return
                "Data Source=C:/Users/Hammer/Desktop/MyProjects/C#/InventoryManagement/InventoryManagement.Core/Data/InventoryDB.db;Version=3;";
        }

        public List<T> LoadData<T, TPrams>(string storedProcedure, TPrams parameters)
        {
            string connectionString = GetConnectionString();

            using IDbConnection db = new SQLiteConnection(connectionString);
            
            var rows = db.Query<T>(storedProcedure, parameters).ToList();

            return rows;
        }

        public void SaveData<T>(string storedProcedure, T parameters)
        {
            string connectionString = GetConnectionString();

            using IDbConnection db = new SQLiteConnection(connectionString);
            db.Execute(storedProcedure, parameters);
        }


        public void StartTransaction()
        {
            string connectionString = GetConnectionString();
            dbConnection = new SQLiteConnection(connectionString);
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
                try
                {
                    CommitTransaction();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            dbTransaction = null;
            dbConnection = null;
        }
    }
}