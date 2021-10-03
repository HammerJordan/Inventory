using Application.Models.Record;

namespace Application.Common.Interfaces
{
    public interface IExportCsvFile
    {
        void ExportToCSV(string path, RecordProductList recordProductList);
    }
}