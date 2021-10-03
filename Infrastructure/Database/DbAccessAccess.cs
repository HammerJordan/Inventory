using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Application.Common.Interfaces;
using Dapper;

namespace Infrastructure.Database
{
    public class DbAccessAccess : IApplicationDbAccess, IDisposable
    {
        public DbAccessAccess(DbConnection connection)
        {
            Connection = new SQLiteConnection(connection.ConnectionString);
            Connection.Open();
        }

        public IDbConnection Connection { get; private set; }

        public List<T> LoadData<T, TPrams>(string storedProcedure, TPrams parameters)
        {
            var rows = Connection.Query<T>(storedProcedure, parameters).ToList();

            return rows;
        }

        public void SaveData<T>(string storedProcedure, T parameters)
        {
            Connection.Execute(storedProcedure, parameters);
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