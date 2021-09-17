using System;
using System.Globalization;

namespace Inventory.Core
{
    public class RecordModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public string CreatedDateTime
        {
            set => CreatedAt = DateTime.Parse(value);
        }

        public string CreatedAtString => CreatedAt.ToString("yyyy/M/dd HH:mm",CultureInfo.InvariantCulture);
    }
}