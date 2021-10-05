using System.Threading;
using System.Threading.Tasks;
using Application.Models.RecordProductList.Queries;
using MediatR;
using Serilog;

namespace Application.Models.RecordProductList.Events.EventHandlers
{
    public abstract class RecordListNotificationHandlerBase<T> : 
        INotificationHandler<T> where T : RecordListNotificationBase
    {
        protected IRecordListItemQuery _query;

        protected RecordListNotificationHandlerBase(IRecordListItemQuery query)
        {
            _query = query;
        }

        public abstract Task Handle(T notification, CancellationToken cancellationToken);
    }
}