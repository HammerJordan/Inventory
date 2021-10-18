using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Common.Interfaces;
using Inventory.Domain.Models;

namespace Application.Core.Models.RecordProductList.Queries
{
    public interface IRecordListItemQuery : IQuery<RecordListItem>
    {
        public Task<IEnumerable<RecordListItem>> LoadAllAsync(RecordModel recordModel);
        public Task<int> Count(RecordModel recordModel);
        public Task DeleteAllAsync(RecordModel recordModel);
    }
}