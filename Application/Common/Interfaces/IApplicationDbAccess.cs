using System.Collections.Generic;
using System.Data;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbAccess
    {
        public IDbConnection Connection { get; }
        List<T> LoadData<T, U>(string query, U parameters);
        void SaveData<T>(string query, T parameters);
    }
}