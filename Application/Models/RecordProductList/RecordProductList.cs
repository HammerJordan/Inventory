using System.Collections.Generic;
using System.Linq;
using Application.Models.RecordProductList.Events;
using Inventory.Application.Common.Exceptions;
using Inventory.Domain.Models;
using MediatR;

namespace Application.Models.RecordProductList
{
    public class RecordProductList
    {
        private readonly IMediator _mediator;
        private readonly List<RecordListItem> _products;
        public RecordModel Record { get; }

        public IReadOnlyList<RecordListItem> ProductLists => _products;

        public RecordProductList(RecordModel record,
            List<RecordListItem> existingRecordItems,
            IMediator mediator)
        {
            Record = record;
            _products = existingRecordItems;
            _mediator = mediator;
        }

        public void Add(ProductModel productModel, int amount = 1)
        {
            var selectedListItem = _products.FirstOrDefault(x => x.ProductID == productModel.ID);

            if (selectedListItem is null)
            {
                selectedListItem = new RecordListItem(productModel, Record) { Quantity = amount };
                _products.Add(selectedListItem);
                _mediator.Publish(new RecordListAddNotification(Record, selectedListItem));
            }
            else
            {
                selectedListItem.Quantity += amount;
                _mediator.Publish(new RecordListUpdateNotification(Record, selectedListItem));
            }

           
        }

        public void SetQuantity(ProductModel productModel, int amount)
        {
            var selectedListItem = _products.FirstOrDefault(x => x.ProductID == productModel.ID);
            if (selectedListItem is null)
                return;
            
            selectedListItem.Quantity = amount;
            _mediator.Publish(new RecordListUpdateNotification(Record, selectedListItem));
            
        }

        public void Subtract(ProductModel productModel, int amount = 1)
        {
            if (_products.All(x => x.ProductID != productModel.ID))
                throw new NotFoundException(productModel.Name);

            var productItem = _products.First(x => x.ProductID == productModel.ID);
            productItem.Quantity -= amount;
            if (productItem.Quantity < 0)
                productItem.Quantity = 0;

            _mediator.Publish(new RecordListUpdateNotification(Record, productItem));
        }

        public void Remove(ProductModel productModel)
        {
            if (_products.All(x => x.ProductID != productModel.ID))
                return;
            var productItem = _products.First(x => x.ProductID == productModel.ID);
            _products.Remove(productItem);

            _mediator.Publish(new RecordListRemoveNotification(Record, productItem));

        }
    }
}