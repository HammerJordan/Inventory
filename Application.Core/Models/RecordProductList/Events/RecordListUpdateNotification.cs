using Inventory.Domain.Models;

namespace Application.Core.Models.RecordProductList.Events
{
    public class RecordListUpdateNotification : RecordListNotificationBase
    {
        public RecordListUpdateNotification(RecordModel recordModel, RecordListItem listItem) 
            : base(recordModel, listItem)
        {
        }
    }
}