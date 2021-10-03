// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using Inventory.Domain.Enums;

namespace Inventory.Domain.Models
{
    public class ProductModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UPC { get; set; }
        public decimal Cost { get; set; }
        public UnitType Unit { get; set; }
        public string URL { get; set; }
        public string ImageHref { get; set; }
        public List<string> CategoryTree { get; set; } = new();
    }
}