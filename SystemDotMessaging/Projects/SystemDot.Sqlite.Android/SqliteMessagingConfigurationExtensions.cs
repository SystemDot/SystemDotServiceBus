using System.IO;
using SystemDot.Sql;
using SystemDot.Sqlite;
using SystemDot.Storage.Changes;
using Mono.Data.Sqlite;

namespace SystemDot.Messaging.Configuration
{
    public static class SqliteMessagingConfigurationExtensions
    {
        const string DatabaseFileName = "MessageStore";
        const string ConnectionString = "Data Source={0}";

        public static MessagingConfiguration UsingSqlitePersistence(this MessagingConfiguration configuration)
        {
            SetupDatabase();
            SetupChangeStore(configuration);

            return configuration;
        }

        static void SetupDatabase()
        {
            string dbFileName = GetDatabaseFilePath();

            CreateDatabaseIfNotExists(dbFileName);
            SetConnectionString(dbFileName);
        }

        static string GetDatabaseFilePath()
        {
            return Path.Combine(GetPersonalFileFolder(), DatabaseFileName);
        }

        static string GetPersonalFileFolder()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }

        static void CreateDatabaseIfNotExists(string toSet)
        {
            if (!File.Exists(toSet)) 
                SqliteConnection.CreateFile(toSet);
        }

        static void SetConnectionString(string databaseFileName)
        {
            DbChangeStore.ConnectionString = string.Format(ConnectionString, databaseFileName);
        }

        static void SetupChangeStore(MessagingConfiguration configuration)
        {
            configuration.GetInternalIocContainer().RegisterInstance<ChangeStore, SqliteChangeStore>();
        }
    }
}