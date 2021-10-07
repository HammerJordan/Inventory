using System;
using Application.WPF.WebScraping.ProductUpdates;
using FluentAssertions;
using Inventory.Domain.Models;
using Microsoft.Extensions.Configuration;
using Xunit;
using Moq;

namespace Application.Tests.Services
{
    public class ProductUpdateRuleTests
    {
        private Mock<IConfiguration> config;
        private ProductUpdateRule _productUpdateRule;
        
        public ProductUpdateRuleTests()
        {
            config = new Mock<IConfiguration>();
            config.Setup(x => x["DaysBetweenUpdates"]).Returns("15");

            _productUpdateRule = new ProductUpdateRule(config.Object);
        }

        [Fact]
        public void ProductsLastUpdatedOlderThenTimeSpanReturnTrue()
        {
            var product = new ProductModel() { LastUpdated = DateTime.Now - TimeSpan.FromDays(18) };
            _productUpdateRule.DoesProductRequireUpdate(product).Should().BeTrue();
        }
        
        [Fact]
        public void ProductsLastUpdatedNewerThenTimeSpanReturnFalse()
        {
            var product = new ProductModel() { LastUpdated = DateTime.Now - TimeSpan.FromDays(14) };
            _productUpdateRule.DoesProductRequireUpdate(product).Should().BeFalse();
        }
    }
}