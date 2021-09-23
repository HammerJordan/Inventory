using Inventory.Desktop.Model;

namespace Inventory.Desktop.Events
{
    public class RecordModelSelect
    {
        public RecordBindableModel Record { get; private set; }

        public RecordModelSelect(RecordBindableModel recordModel)
        {
            Record = recordModel;
        }
    }
}