using Inventory.Domain.Models;
using MediatR;

namespace Application.Core.Models.RecordProductList.Events
{
    public abstract class RecordListNotificationBase : INotification
    {
        public RecordModel RecordModel { get; }
        public RecordListItem ListItem { get; }

        protected RecordListNotificationBase(RecordModel recordModel, RecordListItem listItem)
        {
            RecordModel = recordModel;
            ListItem = listItem;
        }
    }
}