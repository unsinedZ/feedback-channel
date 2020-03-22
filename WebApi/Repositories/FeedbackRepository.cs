using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using WebApi.Projections;
using WebApi.Providers;

namespace WebApi.Repositories
{
    public interface IFeedbackRepository
    {
        Task<int> CreateAsync(FeedbackProjection projection);
        Task<int> DeleteByIdAsync(int id);
        Task<FeedbackProjection> GetByIdAsync(int id);
        Task<IEnumerable<FeedbackProjection>> GetListAsync();
    }

    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly string connectionString;

        public FeedbackRepository(IOptions<ConnectionStringOptions> connectionStringOptions)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string is null or empty.", nameof(connectionString));

            this.connectionString = connectionStringOptions.Value.Default;
        }

        public Task<FeedbackProjection> GetByIdAsync(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                return connection.QuerySingleOrDefaultAsync<FeedbackProjection>(
                    GetByIdQuery(),
                    new { id }
                );
            }
        }

        private string GetByIdQuery()
        {
            return $@"
                select [Id]
                    ,[Subject]
                    ,[Details]
                    ,[Parameters]
                    ,count([Id]) as [TotalCount]
                from [dbo].[Feedbacks]
                where [Id] = @id
            ";
        }

        public Task<IEnumerable<FeedbackProjection>> GetListAsync()
        {
            using (var connection = new MySqlConnection(connectionString))
                return connection.QueryAsync<FeedbackProjection>(GetListQuery());
        }

        private string GetListQuery()
        {
            return $@"
                select [Id]
                    ,[Subject]
                    ,[Details]
                    ,[Parameters]
                from [dbo].[Feedbacks]
            ";
        }

        public Task<int> CreateAsync(FeedbackProjection projection)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                return connection.QuerySingleAsync<int>(
                   GetInsertQuery(),
                   new { feedback = projection });
            }
        }

        private string GetInsertQuery()
        {
            return $@"
                insert into [dbo].[Feedbacks] values (@feedback);
                select cast(SCOPE_IDENTITY() as int);
            ";
        }

        public Task<int> DeleteByIdAsync(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                return connection.ExecuteAsync(
                   "delete from [dbo].[Feedbacks] where [Id] = @id",
                   new { id });
            }
        }
    }
}