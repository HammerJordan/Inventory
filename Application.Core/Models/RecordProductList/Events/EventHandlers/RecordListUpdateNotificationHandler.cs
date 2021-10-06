using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Models.RecordProductList.Queries;
using Serilog;

namespace Application.Core.Models.RecordProductList.Events.EventHandlers
{
    public class RecordListUpdateNotificationHandler : 
        RecordListNotificationHandlerBase<RecordListUpdateNotification>
    {
        public RecordListUpdateNotificationHandler(IRecordListItemQuery query) : base(query)
        {
        }
        
        public override async Task Handle
            (RecordListUpdateNotification notification, CancellationToken cancellationToken)
        {
            try
            {
                await _query.UpdateAsync(notification.ListItem);
            }
            catch (Exception e)
            {
                Log.Fatal(e,"Exception when trying to update list item." +
                                   "item id: {ProductID} record id: {RecordID}",
                    notification.ListItem.ProductID,notification.ListItem.RecordModel.ID);
                throw;
            }
            
            Log.Information("Successfully updated list item" +
                                   "item id: {ProductID} record id: {RecordID}",
                notification.ListItem.ProductID,notification.ListItem.RecordModel.ID);
            
        }




    }
}