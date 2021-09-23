using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Inventory.Core;

namespace Inventory.DataAccess
{
    public sealed class DbConnection : IDisposable
    {
        private readonly SQLiteConnection dbConnection;
        public string ConnectionString { get;  }

        public DbConnection(string pathToDatabase)
        {
            bool newDB = false;
            if(!File.Exists(pathToDatabase))
            {
                CreateNewDatabase(pathToDatabase);
                newDB = true;
            }
            
            ConnectionString = "Data Source=" +
                               $"{pathToDatabase};" +
                               "Version=3;";
            
            
            dbConnection = new SQLiteConnection(ConnectionString);
            dbConnection.Open();
            if (newDB)
                CreateTables();
            dbConnection.Close();
        }

        private void CreateTables()
        {
            dbConnection.Query(TableSchema.CREATE_PRODUCT_SCHEMA);
            dbConnection.Query(TableSchema.CREATE_CATEGORY_SCHEMA);
            dbConnection.Query(TableSchema.CREATE_PRODUCT_IMAGE_SCHEMA);
            dbConnection.Query(TableSchema.CREATE_PRODUCT_CATEGORY_SCHEMA);
            dbConnection.Query(TableSchema.CREATE_RECORD_SCHEMA);
            dbConnection.Query(TableSchema.CREATE_RECORD_ITEM_SCHEMA);
        }

        private static void CreateNewDatabase(string pathToDatabase)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(pathToDatabase) ?? 
                                      throw new InvalidOperationException());
            
            SQLiteConnection.CreateFile(pathToDatabase);
        }

        public async Task InsertProductModel(ProductModel productModel)
        {
            var query = @$"select 1
              from Product
              where Name = '{productModel.Name}'";

            dbConnection.Open();

            var result = await dbConnection.QueryAsync(query);

            //TODO Update the prodcut model in the DB
            if (result.Any())
            {
                await dbConnection.CloseAsync();
                return;
            }

            var sql = $"INSERT INTO Product (Name, Description, UPC, Cost, Unit, URL) " +
                      $"VALUES ('{productModel.Name}','{productModel.Description}','{productModel.UPC}',{productModel.Cost},'{productModel.Unit}','{productModel.URL}')";

            await dbConnection.ExecuteAsync(sql);
            await dbConnection.CloseAsync();
        }

        public async Task InsertProductModels(IEnumerable<ProductModel> models)
        {
            dbConnection.Open();

            ProductModel model;

            try
            {
                foreach (var m in models)
                {
                    model = m;
                    var query = @$" select 1
                                    from Product
                                    where Name = '{m.Name}'";

                    var result = await dbConnection.QueryAsync(query);
                    if (result.Any())
                        continue;

                    var sql = $"INSERT INTO Product (Name, Description, UPC, Cost, Unit, URL) " +
                              $"VALUES (@Name, @Description, @UPC, @Cost, @Unit, @URL)";

                    await dbConnection.ExecuteAsync(sql, new { m.Name, m.Description, m.UPC, m.Cost, m.Unit, m.URL });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                dbConnection.Close();
            }
        }


        public void Dispose()
        {
            dbConnection?.Dispose();
        }
    }
}