using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackApi.Database
{
    class Program
    {
        private const string SchemaPath = "./Schema";
        private const string MigrationsPath = "./Migrations";

        static async Task Main(string[] args)
        {
            var connectionString = args[0];
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Provider connection string.");

            var collector = new ScriptCollector();
            var schemaScriptsTask = collector.ReadAllScriptsAsync(SchemaPath);
            var migrationScriptsTask = collector.ReadAllScriptsAsync(MigrationsPath);
            await Task.WhenAll(schemaScriptsTask, migrationScriptsTask);

            var migrator = new MySqlDatabaseMigrator(connectionString);
            var watch = new Stopwatch();

            Log("Executing schema scripts...");
            var schemaScripts = schemaScriptsTask.Result.ToList();
            watch.Start();

            await migrator.ExecuteMigrationsAsync(schemaScripts);

            watch.Stop();
            Log("Executed {0} schema scripts in {1} ms.", schemaScripts.Count, watch.ElapsedMilliseconds);

            watch.Reset();

            Log("Executing migration scripts...");
            var migrationScripts = migrationScriptsTask.Result;
            watch.Start();

            await migrator.ExecuteMigrationsAsync(migrationScripts);

            watch.Stop();
            Log("Executed {0} migration scripts in {1} ms.", schemaScripts.Count, watch.ElapsedMilliseconds);
        }

        static void Log(string message, params object[] format)
        {
            Console.WriteLine(message, format);
        }
    }
}
