// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;

namespace Inventory.Domain.Models
{
    public class ProductModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UPC { get; set; }
        public decimal Cost { get; set; }
        public string Unit { get; set; }
        public string URL { get; set; }
        public string ImageHref { get; set; }

        public DateTime LastUpdated { get; set; }

        public string LastUpdatedToString => LastUpdated.ToString("yyyy-MM-dd");
        
        public List<string> CategoryTree { get; set; } = new();
    }
}