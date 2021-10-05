using Application.Models.RecordProductList;

namespace Application.Common.Interfaces
{
    public interface IExportCsvFile
    {
        void ExportToCSV(string path, RecordProductList recordProductList);
    }
}