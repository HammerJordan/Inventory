using Inventory.Domain.Models;

namespace Application.Models.RecordProductList.Events
{
    public class RecordListRemoveNotification: RecordListNotificationBase
    {
        public RecordListRemoveNotification(RecordModel recordModel,
            RecordListItem listItem) : base(recordModel,listItem)
        {
        }
    }
}