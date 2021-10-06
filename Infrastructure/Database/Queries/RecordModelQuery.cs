using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Common.Interfaces;
using Application.Core.Models.Record.Queries;
using Infrastructure.Exceptions;
using Inventory.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database.Queries
{
    public class RecordModelQuery : QueryBase, IRecordModelQuery
    {
        public RecordModelQuery(IApplicationDbAccess dbAccess) : base(dbAccess)
        {
        }

        public async Task<RecordModel> GetAsync(int id)

        {
            const string sql = "SELECT * FROM Record WHERE ID == @id LIMIT 1;";

            var awaiter = await _dbAccess.LoadDataAsync<RecordModel, int>(sql, id);

            var result = awaiter.FirstOrDefault();

            if (result == null)
                throw new NotFoundInDbException($"No row found with an ID:{id}");

            return result;
        }

        public async Task<RecordModel> CreateAsync(RecordModel model)
        {
            var sql = @"INSERT INTO Record (Name, CreatedDateTime) 
                            VALUES (@Name,@CreatedDate)";

            var prams = new
            {
                Name = model.Name ?? string.Empty,
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };
            await _dbAccess.SaveDataAsync(sql, prams);

            sql = @"SELECT *
                        FROM Record
                        WHERE ID = (SELECT MAX(ID)  FROM Record);";

            var result = await _dbAccess.LoadDataAsync<RecordModel, dynamic>(sql, null);
            return result.First();
        }

        public async Task UpdateAsync(RecordModel model)
        {
            const string sql = @"UPDATE Record
                            Set Name = @Name
                            Where ID = @ID;";
            await _dbAccess.SaveDataAsync(sql, model);
        }

        public async Task DeleteAsync(RecordModel model)
        {
            const string sql = @"DELETE from Record where ID = @ID;";
            await _dbAccess.SaveDataAsync(sql, model);
        }

        public async Task<IEnumerable<RecordModel>> LoadAllAsync()
        {
            const string sql = "SELECT * FROM Record";
            return await _dbAccess.LoadDataAsync<RecordModel, dynamic>(sql, null);
        }
    }
}