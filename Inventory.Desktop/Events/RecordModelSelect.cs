using Inventory.Domain.Models;
using MediatR;

namespace Inventory.Desktop.Events
{
    public class RecordModelSelectEvent
    {
        public RecordModel Record { get; }

        public RecordModelSelectEvent(RecordModel recordModel)
        {
            Record = recordModel;
        }
    }
}