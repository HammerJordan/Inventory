using Inventory.Desktop.Model;

namespace Inventory.Desktop.Events
{
    public class RecordModelSelect
    {
        public RecordViewModel Record { get; private set; }

        public RecordModelSelect(RecordViewModel recordModel)
        {
            Record = recordModel;
        }
    }
}