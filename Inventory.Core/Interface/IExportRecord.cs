using System.Collections.Generic;

namespace Inventory.Core
{
    public interface IExportRecord
    {
        void ExportToCSV(string path, RecordModel model, IEnumerable<ProductModel> product);
    }
}