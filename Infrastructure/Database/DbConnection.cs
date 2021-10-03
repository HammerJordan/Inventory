using System;
using System.Data.SQLite;
using System.IO;
using Dapper;

namespace Infrastructure.Database
{
    public class DbConnection : IDisposable
    {
        private readonly SQLiteConnection _dbConnection;
        public string ConnectionString { get; }

        public DbConnection(string pathToDatabase)
        {
            bool newDB = false;
            if (!File.Exists(pathToDatabase))
            {
                CreateNewDatabase(pathToDatabase);
                newDB = true;
            }

            ConnectionString = "Data Source=" +
                               $"{pathToDatabase};" +
                               "Version=3;";

            _dbConnection = new SQLiteConnection(ConnectionString);
            _dbConnection.Open();
            if (newDB)
                CreateTables();
            _dbConnection.Close();
        }

        private void CreateTables()
        {
            _dbConnection.Query(TableSchema.CREATE_PRODUCT_SCHEMA);
            _dbConnection.Query(TableSchema.CREATE_CATEGORY_SCHEMA);
            _dbConnection.Query(TableSchema.CREATE_PRODUCT_CATEGORY_SCHEMA);
            _dbConnection.Query(TableSchema.CREATE_RECORD_SCHEMA);
            _dbConnection.Query(TableSchema.CREATE_RECORD_ITEM_SCHEMA);
        }

        private static void CreateNewDatabase(string pathToDatabase)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(pathToDatabase) ??
                                      throw new InvalidOperationException());

            SQLiteConnection.CreateFile(pathToDatabase);
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}