using Application.Core.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database.Queries
{
    public abstract class QueryBase
    {
        protected readonly IApplicationDbAccess _dbAccess;

        protected QueryBase(IApplicationDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }
    }
}