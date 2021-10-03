using Application.Common.Interfaces;

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