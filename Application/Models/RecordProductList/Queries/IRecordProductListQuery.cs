using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Inventory.Domain.Models;

namespace Application.Models.RecordProductList.Queries
{
    public interface IRecordListItemQuery : IQuery<RecordListItem>
    {
        public Task<IEnumerable<RecordListItem>> LoadAllAsync(RecordModel recordModel);
    }
}