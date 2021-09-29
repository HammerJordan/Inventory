using Inventory.Core;

namespace Inventory.Desktop.Events
{
    public class RecordModelSelect
    {
        public RecordModel Record { get; private set; }

        public RecordModelSelect(RecordModel recordModel)
        {
            Record = recordModel;
        }
    }
}