using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using SQLite;

namespace SystemDot.Messaging.Storage.Sqlite.Metro
{
    public class SqliteChangeStore : IChangeStore
    {
        readonly ISerialiser serialiser;
        string databasePath;

        public SqliteChangeStore(ISerialiser serialiser)
        {
            this.serialiser = serialiser;
        }

        public async void Initialise(string connection)
        {
            this.databasePath = connection;

            await GetAsyncConnection().ExecuteAsync(
                "create table if not exists ChangeStore(\n"
                + "Id varchar(32) not null, \n"
                + "ChangeRootId varchar(1000) not null, \n"
                + "Change blob, \n"
                + "Sequence integer PRIMARY KEY)");
        }

        public Guid StoreChange(string changeRootId, Change change)
        {
            Guid id = Guid.NewGuid();

            GetConnection().Execute(
                "insert into ChangeStore(Id, ChangeRootId, Change) values(?, ?, ?)", 
                id.ToString(),
                changeRootId,
                this.serialiser.Serialise(change));

            return id;
        }

        public IEnumerable<Change> GetChanges(string changeRootId)
        {
            return GetChangesAsync(changeRootId)
                .Result
                .Select(c => this.serialiser.Deserialise(c.Change).As<Change>())
                .ToList();
        }

        async Task<IEnumerable<SqliteChangeContainer>> GetChangesAsync(string changeRootId)
        {
            return await GetAsyncConnection()
                .Table<SqliteChangeContainer>()
                .Where(m => m.ChangeRootId == changeRootId)
                .ToListAsync();
        }

        public Change GetChange(Guid id)
        {
            return this.serialiser
                .Deserialise(GetChangeAsync(id).Result.Change)
                .As<Change>();
        }

        async Task<SqliteChangeContainer> GetChangeAsync(Guid id)
        {
            return await GetAsyncConnection()
                .Table<SqliteChangeContainer>()
                .Where(m => m.Id == id.ToString())
                .FirstAsync();
        }

        SQLiteAsyncConnection GetAsyncConnection()
        {
            return new SQLiteAsyncConnection(databasePath);
        }

        SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(databasePath);
        }
    }
}
