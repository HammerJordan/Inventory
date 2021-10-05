using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.RecordProductList.Queries;
using Serilog;

namespace Application.Models.RecordProductList.Events.EventHandlers
{
    public class RecordListRemoveNotificationHandler : RecordListNotificationHandlerBase<RecordListRemoveNotification>
    {
        public RecordListRemoveNotificationHandler(IRecordListItemQuery query) : base(query)
        {
        }

        public override async Task Handle(RecordListRemoveNotification notification,
            CancellationToken cancellationToken)
        {
            try
            {
                await _query.DeleteAsync(notification.ListItem);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Exception when trying to Delete list item." +
                                    "item id: {ProductID} record id: {RecordID}",
                    notification.ListItem.ProductID, notification.ListItem.RecordModel.ID);
                throw;
            }

            Log.Information("Successfully Deleted list item" +
                                   "item id: {ProductID} record id: {RecordID}",
                notification.ListItem.ProductID, notification.ListItem.RecordModel.ID);
        }
    }
}