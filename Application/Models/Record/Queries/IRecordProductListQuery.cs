using System.Collections.Generic;
using Application.Common.Interfaces;
using Inventory.Domain.Models;

namespace Application.Models.Record.Queries
{
    public interface IRecordListItemQuery : IQuery<RecordListItem>
    {
    public IEnumerable<RecordListItem> LoadAll(RecordModel recordModel);
    }
}