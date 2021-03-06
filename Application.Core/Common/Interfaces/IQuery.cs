using System.Threading.Tasks;

namespace Application.Core.Common.Interfaces
{
    public interface IQuery<T>
    {
        Task<T> CreateAsync(T model);
        Task UpdateAsync(T model);
        Task DeleteAsync(T model);
    }
}