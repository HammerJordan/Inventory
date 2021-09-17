using System.Collections.Generic;

namespace Inventory.DataAccess
{
    public interface ISqlLiteDataAccess
    {
        string GetConnectionString();
        List<T> LoadData<T, U>(string storedProcedure, U parameters);
        void SaveData<T>(string storedProcedure, T parameters);

        void StartTransaction();
        void SaveDataInTransaction<T>(string storedProcedure, T parameters);
        List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters);
        void CommitTransaction();
        void RollbackTransaction();
    }
}