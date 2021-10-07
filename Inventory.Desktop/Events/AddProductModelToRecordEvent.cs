using Inventory.Desktop.ViewModel;
using Inventory.Domain.Models;
using MediatR;

namespace Inventory.Desktop.Events
{
    public class AddProductModelToRecordEvent
    {
        public ProductViewModel Model { get; }

        public AddProductModelToRecordEvent(ProductViewModel model)
        {
            Model = model;
        }
    }
}