using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Common.Interfaces;
using Application.Core.Models.Record;
using Application.Core.Models.Record.Queries;
using Application.Core.Models.RecordProductList;
using Application.Core.Models.RecordProductList.Queries;
using Inventory.Domain.Models;

namespace Infrastructure.FileAccess
{
    public class ExportRecordListToCsv : IExportCsvFile
    {
        private readonly IRecordListItemQuery _listItemQuery;

        public ExportRecordListToCsv(IRecordListItemQuery listItemQuery)
        {
            _listItemQuery = listItemQuery;
        }

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

        public async Task ExportToCSV(string path, string fileName, IEnumerable<RecordModel> recordModels)
        {
            string fullFilePath = Path.Join(path, $"{fileName}.csv");
            string[] columnNames = new[] { "Record", "Name", "Cost", "Quantity", "Total\n" };


            await File.WriteAllTextAsync(fullFilePath, string.Join(",",columnNames));

            string[] rowToWrite = new string[5];

            foreach (var record in recordModels)
            {
                var items = await _listItemQuery.LoadAllAsync(record);

                foreach (var item in items)
                {
                    rowToWrite[0] = record.Name;
                    rowToWrite[1] = item.Name;
                    rowToWrite[2] = item.Cost.ToString();
                    rowToWrite[3] = item.Quantity.ToString();
                    rowToWrite[4] = (item.Quantity * item.Cost).ToString() + '\n';

                    await File.AppendAllTextAsync(fullFilePath, string.Join(",", rowToWrite));
                }
            }
        }
    }
}