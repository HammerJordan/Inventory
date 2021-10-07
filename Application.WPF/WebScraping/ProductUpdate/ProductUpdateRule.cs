using System;
using Inventory.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Application.WPF.WebScraping.ProductUpdates
{
    public class ProductUpdateRule
    {
        private readonly TimeSpan _timeBetweenUpdates;

        public ProductUpdateRule(IConfiguration configuration)
        {
            var dayString = configuration["DaysBetweenUpdates"];
            if (string.IsNullOrEmpty(dayString) || !int.TryParse(dayString, out int days))
                days = 15;
            _timeBetweenUpdates = new TimeSpan(days,0,0,0);
        }

        public bool DoesProductRequireUpdate(ProductModel model)
        {
            var duration = DateTime.Now - model.LastUpdated;
            return duration > _timeBetweenUpdates;
        }
    }
}