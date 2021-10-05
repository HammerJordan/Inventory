using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.RecordProductList.Queries;
using Serilog;

namespace Application.Models.RecordProductList.Events.EventHandlers
{
    public class RecordListAddNotificationHandler : 
        RecordListNotificationHandlerBase<RecordListAddNotification>
    {
        
        public RecordListAddNotificationHandler
            (IRecordListItemQuery query) : base(query)
        {
        }


        public override async Task Handle(RecordListAddNotification notification,
            CancellationToken cancellationToken)
        {
            try
            {
                await _query.CreateAsync(notification.ListItem);
            }
            catch (Exception e)
            {
                Log.Fatal(e,"Exception when trying to crate list item." +
                                   "item id: {ProductID} record id: {RecordID}",
                    notification.ListItem.ProductID,notification.ListItem.RecordModel.ID);
                throw;
            }
            
            Log.Information("Successfully created list item" +
                               "item id: {ProductID} record id: {RecordID}",
                notification.ListItem.ProductID,notification.ListItem.RecordModel.ID);
            
        }


    }
}