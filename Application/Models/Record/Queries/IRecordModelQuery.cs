using System.Collections.Generic;
using Application.Common.Interfaces;
using Inventory.Domain.Models;

namespace Application.Models.Record.Queries
{
    public interface IRecordModelQuery : IQuery<RecordModel>
    {
        public IEnumerable<RecordModel> LoadAll();
    }
}