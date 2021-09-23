// ReSharper disable InconsistentNaming

using System;
using System.Drawing;

namespace Inventory.Core
{
    public class ProductModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UPC { get; set; }
        public double Cost { get; set; }
        public string Unit { get; set; }
        public string URL { get; set; }
        public string Category { get; set; }
        
        public string ImageHref { get; set; }

        public int Quantity { get; set; } = 1;


        public string Group { get; set; }
        public string SubGroup { get; set; }

        public string NowToString => DateTime.UtcNow.ToShortDateString();
        //TODO Image
    }
}