using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Inventory.Domain.Models;

namespace Application.Models.Record.Queries
{
    public interface IRecordModelQuery : IQuery<RecordModel>
    {
        public Task<RecordModel> GetAsync(int id);
        public Task<IEnumerable<RecordModel>> LoadAllAsync();
    }
}