using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbAccess
    {
        public IDbConnection Connection { get; }
        Task<List<T>> LoadDataAsync<T, U>(string query, U parameters);
        Task SaveDataAsync<T>(string query, T parameters);
    }
}