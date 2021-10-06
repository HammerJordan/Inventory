using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Common.Interfaces;
using Dapper;

namespace Infrastructure.Database
{
    public class DbAccess : IApplicationDbAccess, IDisposable
    {
        public DbAccess(DbConnection connection)
        {
            Connection = new SQLiteConnection(connection.ConnectionString);
            Connection.Open();
        }

        public IDbConnection Connection { get; private set; }

        public async Task<List<T>> LoadDataAsync<T, TPrams>(string storedProcedure, TPrams parameters)
        {
            var rows = await Connection.QueryAsync<T>(storedProcedure, parameters);

            return rows.ToList();
        }

        public async Task SaveDataAsync<T>(string storedProcedure, T parameters)
        {
            await Connection.ExecuteAsync(storedProcedure, parameters);
        }

        public void Dispose()
        {
            CommitTransaction();

            Connection = null;
        }

        public void CommitTransaction()
        {
            Connection?.Close();
        }
    }
}