using System;
using System.IO;
using System.Linq;
using Application.Common.Interfaces;
using Application.Models.Record;

namespace Infrastructure.FileAccess
{
    public class ExportRecordListToCsv : IExportCsvFile
    {
        public void ExportToCSV(string path, RecordProductList recordProductList)
        {
            string fileName = $"({recordProductList.Record.Name})[{DateTime.Now}].csv";
            fileName = fileName.Replace(':', ' ');

            var lines = recordProductList.ProductLists
                .Select(x => $"{x.Name},{x.Cost},{x.Quantity},{x.Cost * x.Quantity}")
                .ToList();

            lines.Insert(0,"Name, Cost, Quantity, Total");

            File.WriteAllLines(Path.Join(path, fileName), lines);
        }
    }
}