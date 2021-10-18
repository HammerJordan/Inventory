using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Models.RecordProductList;
using Inventory.Domain.Models;

namespace Application.Core.Common.Interfaces
{
    public interface IExportCsvFile
    { 
        Task ExportToCSV(string path, string fileName, IEnumerable<RecordModel> recordModels);
    }

}