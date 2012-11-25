using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using Microsoft.Isam.Esent.Interop;

namespace SystemDot.Messaging.Storage.Esent
{
    public class EsentChangeStore : Disposable, IChangeStore
    {
        const string DatabaseName = "Messaging";
        const string InstanceName = "Instance";
        
        readonly IFileSystem fileSystem;
        readonly ISerialiser serialiser;
        
        Instance instance;

        public EsentChangeStore(IFileSystem fileSystem, ISerialiser serialiser)
        {
            Contract.Requires(fileSystem != null);
            Contract.Requires(serialiser != null);

            this.fileSystem = fileSystem;
            this.serialiser = serialiser;
        }

        public void Initialise(string connection)
        {
            this.instance = new Instance(InstanceName + Guid.NewGuid());
            instance.Init();

            using (var session = new Session(this.instance))
                if (!this.fileSystem.FileExists(DatabaseName)) 
                    CreateDatabaseAndTables(session, DatabaseName);            
        }

        static void CreateDatabaseAndTables(Session session, string databaseName)
        {
            CreateTables(Esent.CreateDatabase(session, databaseName), session);
        }

        static void CreateTables(JET_DBID dbId, JET_SESID session)
        {
            using (var transaction = new Transaction(session))
            {
                JET_TABLEID tableId = Esent.CreateTable(dbId, session, ChangeStoreTable.Name);

                Esent.AddColumn(session, tableId, ChangeStoreTable.IdColumn, JET_coltyp.Binary, JET_CP.None);
                Esent.AddColumn(session, tableId, ChangeStoreTable.SequenceColumn, JET_coltyp.Long, JET_CP.None, ColumndefGrbit.ColumnAutoincrement);
                Esent.AddColumn(session, tableId, ChangeStoreTable.ChangeRootIdColumn, JET_coltyp.Text, JET_CP.Unicode);
                Esent.AddColumn(session, tableId, ChangeStoreTable.BodyColumn, JET_coltyp.LongText, JET_CP.Unicode);
                Esent.CreatePrimaryIndex(session, tableId, ChangeStoreTable.PrimaryIndex, ChangeStoreTable.SequenceColumn);
                Esent.CreateIndex(session, tableId, ChangeStoreTable.ChangeRootIndex, ChangeStoreTable.ChangeRootIdColumn);
                Esent.CreateIndex(session, tableId, ChangeStoreTable.IdIndex, ChangeStoreTable.IdColumn);
                
                Esent.CloseTable(session, tableId);

                transaction.Commit(CommitTransactionGrbit.None);
            }
        }

        public Guid StoreChange(string changeRootId, Change change)
        {
            var id = Guid.NewGuid();

            using (var session = new Session(this.instance))
                StoreChange(session, id, changeRootId, change);            

            return id;
        }

        void StoreChange(Session session, Guid id, string changeRootId, Change change)
        {
            JET_DBID dbId = Esent.OpenDatabase(session, DatabaseName);

            using (var table = new Table(session, dbId, ChangeStoreTable.Name, OpenTableGrbit.None))
            {
                var columns = Esent.GetColumns(session, table);

                using (var transaction = new Transaction(session))
                {
                    StoreChange(session, table, columns, id, changeRootId, change);
                    transaction.Commit(CommitTransactionGrbit.None);
                }
            }
        }

        void StoreChange(Session session, Table table, IDictionary<string, JET_COLUMNID> columns, Guid id, string changeRootId, Change change)
        {
            using (var update = new Update(session, table, JET_prep.Insert))
            {
                Esent.SetColumn(session, table, columns[ChangeStoreTable.IdColumn], id);
                Esent.SetColumn(session, table, columns[ChangeStoreTable.ChangeRootIdColumn], changeRootId, Encoding.Unicode);
                Esent.SetColumn(session, table, columns[ChangeStoreTable.BodyColumn], this.serialiser.Serialise(change));

                update.Save();
            }
        }

        public IEnumerable<Change> GetChanges(string changeRootId)
        {                    
            using (var session = new Session(this.instance))
            {
                JET_DBID dbId = Esent.OpenDatabase(session, DatabaseName);

                using (var table = new Table(session, dbId, ChangeStoreTable.Name, OpenTableGrbit.None))
                using (new Transaction(session))
                    return GetChanges(changeRootId, session, table);                
            }
        }

        IEnumerable<Change> GetChanges(string changeRootId, Session session, Table table)
        {
            var changes = new List<Change>();
            IDictionary<string, JET_COLUMNID> columns = Esent.GetColumns(session, table);

            Esent.UseIndex(session, table, ChangeStoreTable.ChangeRootIndex);
            
            Esent.SetSearchKey(session, table, changeRootId, Encoding.Unicode);
            if (!Esent.TrySearchForEqualToKey(session, table)) return changes;
            
            Esent.SetSearchKey(session, table, changeRootId, Encoding.Unicode);
            Esent.SetIndexRange(session, table);

            while (Esent.TryMoveNext(session, table))
                changes.Add(GetChangeFromColumn(session, table, columns[ChangeStoreTable.BodyColumn]));                
            
             return changes;
        }

        public Change GetChange(Guid id)
        {
            using (var session = new Session(this.instance))
            {
                JET_DBID dbId = Esent.OpenDatabase(session, DatabaseName);

                using (var table = new Table(session, dbId, ChangeStoreTable.Name, OpenTableGrbit.None))
                    using (new Transaction(session))
                        return GetChange(id, session, table);                  
            }
        }

        Change GetChange(Guid id, Session session, Table table)
        {
            Esent.UseIndex(session, table, ChangeStoreTable.IdIndex);
            Esent.SetSearchKey(session, table, id);
            Esent.SearchForEqualToKey(session, table);

            return GetChangeFromColumn(
                session,
                table,
                Esent.GetColumns(session, table)[ChangeStoreTable.BodyColumn]);
        }

        Change GetChangeFromColumn(Session session, Table table, JET_COLUMNID column)
        {
            return this.serialiser
                .Deserialise(Esent.RetrieveBytesFromColumn(session, table, column))
                .As<Change>();
        }

        protected override void DisposeOfManagedResources()
        {
            this.instance.Dispose();
            base.DisposeOfManagedResources();
        }
    }
}