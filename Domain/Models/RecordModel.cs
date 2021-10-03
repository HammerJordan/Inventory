using System;
using System.Globalization;

// ReSharper disable InconsistentNaming

namespace Inventory.Domain.Models
{
    public class RecordModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedAtString => CreatedAt.ToString("yyyy/M/dd HH:mm", CultureInfo.InvariantCulture);
    }
}