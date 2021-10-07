using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Common.Interfaces;
using Application.Core.Models.Product.Queries;
using Dapper;
using Inventory.Domain.Models;

namespace Infrastructure.Database.Queries
{
    public class ProductModelQuery : QueryBase, IProductModelQuery
    {
        public ProductModelQuery(IApplicationDbAccess dbAccess) : base(dbAccess)
        {
        }

        public async Task<ProductModel> CreateAsync(ProductModel model)
        {
            const string sql = @"
                INSERT INTO Product
                    (Name, Description, UPC, Cost, Unit, URL, LastUpdated,ImageHref)
                VALUES
                    (@Name, @Description, @UPC, @Cost, @Unit, @URL,@LastUpdatedToString,@ImageHref)";

            model.LastUpdated = DateTime.Now;

            await _dbAccess.SaveDataAsync(sql, model);

            return await FindByNameAsync(model.Name);
        }

        public async Task UpdateAsync(ProductModel model)
        {
            const string sql = @"UPDATE Product
                                SET
                                    Description = @Description,
                                    UPC = @UPC,
                                    Cost = @Cost,
                                    URL = @URL,
                                    LastUpdated = @LastUpdatedToString,
                                    ImageHref = @ImageHref
                                        WHERE ID = @ID;";

            model.LastUpdated = DateTime.Now;

            await _dbAccess.SaveDataAsync(sql, model);
        }

        public async Task DeleteAsync(ProductModel model)
        {
            const string sql = @"DELETE FROM Product WHERE ID = @ID";
            await _dbAccess.SaveDataAsync(sql, model);
        }

        public async Task<ProductModel> FindByNameAsync(string name)
        {
            string sql = @"SELECT * FROM Product WHERE Name = @name;";
            var result = await _dbAccess.Connection.QueryAsync<ProductModel>(sql, new { name });
            return result.FirstOrDefault();
        }
    }
}