using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Application.Models.RecordProductList;
using Application.Models.RecordProductList.Events;
using FluentAssertions;
using Inventory.Application.Common.Exceptions;
using Inventory.Domain.Models;
using MediatR;
using Moq;
using Xunit;

namespace Application.Tests.Models
{
    public class RecordProductListTests
    {
        private readonly RecordProductList _recordProductList;
        private RecordModel _recordModel;
        private Mock<IMediator> _mediator;
        private List<RecordListItem> _existingRecordItems;

        public RecordProductListTests()
        {
            _mediator = new Mock<IMediator>();
            _recordModel = new RecordModel() { ID = 1 };
            _existingRecordItems = new List<RecordListItem>();
            _recordProductList = new RecordProductList(_recordModel, _existingRecordItems, _mediator.Object);
        }

        [Fact]
        public void Add_WhenItIsNotAlreadyAdded()
        {
            var product = new ProductModel() { ID = 1 };

            _recordProductList.Add(product);

            _recordProductList.ProductLists
                .Should().HaveCount(1)
                .And.ContainSingle(x => x.ProductID == product.ID);
        }

        [Fact]
        public void AddMultiple_ShouldAddNewProduct_WhenItIsNotAlreadyAdded()
        {
            var product = new ProductModel() { ID = 1 };

            _recordProductList.Add(product, 5);

            _recordProductList.ProductLists.Should().HaveCount(1)
                .And.ContainSingle(x => x.ProductID == product.ID)
                .And.Subject.First().Quantity.Should().Be(5);
        }

        [Fact]
        public void AddMultiple_ShouldIncrementQuantity_WhenItIsNotAlreadyAdded()
        {
            var product = new ProductModel() { ID = 1 };
            _recordProductList.Add(product);

            _recordProductList.Add(product);

            _recordProductList.ProductLists.Should().HaveCount(1)
                .And.ContainSingle(x => x.ProductID == product.ID)
                .And.Subject.First().Quantity.Should().Be(2);
        }

        [Fact]
        public void Subtract_ShouldDecrementsQuantity_WhenQuantityIsGraterThenZero()
        {
            var product = new ProductModel() { ID = 1 };

            _recordProductList.Add(product);
            _recordProductList.Add(product);

            _recordProductList.Subtract(product);

            _recordProductList.ProductLists.Should().HaveCount(1)
                .And.ContainSingle(x => x.ProductID == product.ID)
                .And.Subject.First().Quantity.Should().Be(1);
        }

        [Fact]
        public void Subtract_ShouldNotGoBelowZero()
        {
            var product = new ProductModel() { ID = 1 };

            _recordProductList.Add(product);
            _recordProductList.Add(product);

            _recordProductList.Subtract(product, 3);

            _recordProductList.ProductLists.Should().HaveCount(1)
                .And.ContainSingle(x => x.ProductID == product.ID)
                .And.Subject.First().Quantity.Should().Be(0);
        }

        [Fact]
        public void Subtract_ShouldThrowNotFoundException_WhenProductIsNotAdded()
        {
            var product = new ProductModel() { ID = 1 };
            var action = new Action(() => _recordProductList.Subtract(product, 3));

            action.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void Remove_ShouldRemove()
        {
            var product = new ProductModel() { ID = 1 };
            _recordProductList.Add(product);

            _recordProductList.Remove(product);

            _recordProductList.ProductLists.Should().HaveCount(0);
        }

        [Fact]
        public void Add_ShouldPublishAddedNotification()
        {
            var product = new ProductModel() { ID = 1 };
            RecordListNotificationBase publishedAddNotification = null;
            
            _mediator
                .Setup(x => x.Publish(It.IsAny<RecordListNotificationBase>(), CancellationToken.None))
                .Callback<RecordListNotificationBase, 
                    CancellationToken>((x, _) => publishedAddNotification = x);

            _recordProductList.Add(product);

            publishedAddNotification.ListItem.ProductID.Should().Be(product.ID);
            publishedAddNotification.RecordModel.ID.Should().Be(_recordModel.ID);
            publishedAddNotification.Should().BeOfType<RecordListAddNotification>();
            
            _recordProductList.Add(product);
            publishedAddNotification.ListItem.Quantity.Should().Be(2);
            publishedAddNotification.Should().BeOfType<RecordListUpdateNotification>();
        }
        
        [Fact]
        public void Subtract_ShouldPublishSubtractNotification()
        {
            var product = new ProductModel() { ID = 1 };
            RecordListUpdateNotification publishedNotification = null;
            
            _mediator
                .Setup(x => x.Publish(It.IsAny<RecordListUpdateNotification>(), CancellationToken.None))
                .Callback<RecordListUpdateNotification, CancellationToken>((x, y) => publishedNotification = x);

            _recordProductList.Add(product,3);
            
            _recordProductList.Subtract(product);
            
            publishedNotification.ListItem.Quantity.Should().Be(2);
            publishedNotification.ListItem.ProductID.Should().Be(product.ID);
        }
        
        [Fact]
        public void Remove_ShouldPublishRemoveNotification()
        {
            var product = new ProductModel() { ID = 1 };
            RecordListRemoveNotification publishedNotification = null;
            
            _mediator
                .Setup(x => x.Publish(It.IsAny<RecordListRemoveNotification>(), CancellationToken.None))
                .Callback<RecordListRemoveNotification, CancellationToken>((x, y) => publishedNotification = x);

            _recordProductList.Add(product,3);
            
            _recordProductList.Remove(product);
            
            publishedNotification.ListItem.ProductID.Should().Be(product.ID);
        }
        
    }
}