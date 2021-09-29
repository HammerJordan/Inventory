using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;

namespace Inventory.Core.Services
{
    public class ExportRecord : IExportRecord
    {
        public void ExportToCSV(string path, RecordModel model, IEnumerable<ProductModel> product)
        {
            string fileName = $"({model.Name})[{DateTime.Now}].csv";
            fileName = fileName.Replace(':', ' ');

            var lines = product
                .Select(x => $"{x.Name},{x.Cost},{x.Quantity},{x.Cost * x.Quantity}")
                .ToList();

            lines.Insert(0,"Name, Cost, Quantity, Total");
            


            File.WriteAllLines(Path.Join(path, fileName), lines);
        }
    }
}