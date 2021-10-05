using Inventory.Domain.Models;

namespace Application.Models.RecordProductList.Events
{
    public class RecordListAddNotification : RecordListNotificationBase
    {
        public RecordListAddNotification(RecordModel recordModel,
            RecordListItem listItem) : base(recordModel, listItem)
        {
        }
    }
}