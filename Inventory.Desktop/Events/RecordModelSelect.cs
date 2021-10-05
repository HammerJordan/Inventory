using Inventory.Domain.Models;

namespace Inventory.Desktop.Events
{
    public class RecordModelSelect
    {
        public RecordModel Record { get; }

        public RecordModelSelect(RecordModel recordModel)
        {
            Record = recordModel;
        }
    }
}