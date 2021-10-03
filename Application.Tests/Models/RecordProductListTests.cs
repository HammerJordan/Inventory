using System;
using System.Linq;
using Application.Models.Record;
using FluentAssertions;
using Inventory.Application.Common.Exceptions;
using Inventory.Domain.Models;
using Xunit;

namespace Application.Tests.Models
{
    public class RecordProductListTests
    {
        private RecordProductList _productList;
        private RecordModel _recordModel;
        
        public RecordProductListTests()
        {
            _recordModel = new RecordModel() { ID = 1 };
            _productList = new RecordProductList(_recordModel);
        }
        
        [Fact]
        public void Add_ShouldAddNewProduct_WhenItIsNotAlreadyAdded()
        {
            var product = new ProductModel() { ID = 1 };
            
            _productList.Add(product);

            _productList.ProductLists.Should().HaveCount(1).And.ContainSingle(x => x.ProductID == product.ID);
        }
        
        [Fact]
        public void AddMultiple_ShouldAddNewProduct_WhenItIsNotAlreadyAdded()
        {
            var product = new ProductModel() { ID = 1 };
            
            _productList.Add(product,5);

            _productList.ProductLists.Should().HaveCount(1)
                .And.ContainSingle(x => x.ProductID == product.ID)
                .And.Subject.First().Quantity.Should().Be(5);
        }
        
        [Fact]
        public void AddMultiple_ShouldIncrementQuantity_WhenItIsNotAlreadyAdded()
        {
            var product = new ProductModel() { ID = 1 };
            
            _productList.Add(product);
            _productList.Add(product);

            _productList.ProductLists.Should().HaveCount(1)
                .And.ContainSingle(x => x.ProductID == product.ID)
                .And.Subject.First().Quantity.Should().Be(2);
        }
        
        [Fact]
        public void Subtract_ShouldDecrementsQuantity_WhenQuantityIsGraterThenZero()
        {
            var product = new ProductModel() { ID = 1 };
            
            _productList.Add(product);
            _productList.Add(product);
            
            _productList.Subtract(product);

            _productList.ProductLists.Should().HaveCount(1)
                .And.ContainSingle(x => x.ProductID == product.ID)
                .And.Subject.First().Quantity.Should().Be(1);
        }
        
        [Fact]
        public void Subtract_ShouldNotGoBelowZero()
        {
            var product = new ProductModel() { ID = 1 };
            
            _productList.Add(product);
            _productList.Add(product);
            
            _productList.Subtract(product, 3);

            _productList.ProductLists.Should().HaveCount(1)
                .And.ContainSingle(x => x.ProductID == product.ID)
                .And.Subject.First().Quantity.Should().Be(0);
        }
        
        [Fact]
        public void Subtract_ShouldThrowNotFoundException_WhenProductIsNotAdded()
        {
            var product = new ProductModel() { ID = 1 };
            var action = new Action(() => _productList.Subtract(product, 3));

            action.Should().Throw<NotFoundException>();

        }       
        
        
        [Fact]
        public void Remove_ShouldRemove()
        {
            var product = new ProductModel() { ID = 1 };
            _productList.Add(product);
            
            _productList.Remove(product);

            _productList.ProductLists.Should().HaveCount(0);
        }
    }
}