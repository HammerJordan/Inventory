using System.Collections.Generic;

namespace Inventory.Core
{
    public interface IExportRecord
    {
        void ExportToCSV(RecordModel model, IEnumerable<ProductModel> product);
    }
}