using System.Threading.Tasks;

namespace Inventory.DataAccess.StoreProcedure
{
    public class DbStoredProcedure
    {
        //TODO
        private readonly ISqlLiteDataAccess dataAccess;

        public DbStoredProcedure(ISqlLiteDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        // public TResult Query<TResult,TArgs>(IStoreProcedure storeProcedure, TArgs args)
        // {
        //     return default TResult;
        // }
        
    }
}