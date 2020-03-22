using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;

namespace FeedbackApi.Database
{
    public class MySqlDatabaseMigrator
    {
        private readonly string connectionString;

        public MySqlDatabaseMigrator(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task ExecuteMigrationsAsync(IEnumerable<string> migrations)
        {
            if (migrations is null)
                throw new ArgumentNullException(nameof(migrations));

            var databaseName = new MySqlConnectionStringBuilder(connectionString).Database;
            var command = CombineMigrations(migrations).Replace("%DBNAME%", databaseName);
            using (var connection = new MySqlConnection(connectionString))
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    await connection.ExecuteAsync(command, transaction: transaction);
                    await transaction.CommitAsync();
                }
            }
        }

        private string CombineMigrations(IEnumerable<string> migrations)
        {
            var stringBuilder = new StringBuilder();
            foreach (var migration in migrations)
            {
                if (!string.IsNullOrWhiteSpace(migration))
                {
                    stringBuilder.AppendLine(migration + ";");
                    stringBuilder.AppendLine("GO;");
                }
            }

            return stringBuilder.ToString().Trim();
        }
    }
}