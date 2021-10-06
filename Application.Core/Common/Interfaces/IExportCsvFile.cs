using Application.Core.Models.RecordProductList;

namespace Application.Core.Common.Interfaces
{
    public interface IExportCsvFile
    {
        void ExportToCSV(string path, RecordProductList recordProductList);
    }
}