using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Models.Record.Queries;
using Application.Core.Models.RecordProductList.Queries;
using Inventory.Domain.Models;
using MediatR;

namespace Inventory.Application.Core.Models.Record.Commands
{
    public class DeleteRecordCommand : IRequest
    {
        public RecordModel Record { get; private set; }

        public DeleteRecordCommand(RecordModel recordModel)
        {
            Record = recordModel;
        }
    }

    public class DeleteRecordCommandHandler : IRequestHandler<DeleteRecordCommand>
    {
        private readonly IRecordModelQuery _recordModelQuery;
        private readonly IRecordListItemQuery _recordListItemQuery;

        public DeleteRecordCommandHandler(IRecordModelQuery recordModelQuery, IRecordListItemQuery recordListItemQuery)
        {
            _recordModelQuery = recordModelQuery;
            _recordListItemQuery = recordListItemQuery;
        }

        public async Task<Unit> Handle(DeleteRecordCommand request, CancellationToken cancellationToken)
        {
            var record = request.Record;

            await _recordListItemQuery.DeleteAllAsync(record);
            await _recordModelQuery.DeleteAsync(record);

            return new Unit();
        }
    }
}