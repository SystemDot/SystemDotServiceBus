using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using System.Text;
using SystemDot.Files;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
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
            this.instance = new Instance(InstanceName);
            this.instance.Parameters.MaxVerPages = 1024;
            instance.Init();

            using (var session = new Session(this.instance))
                if (!this.fileSystem.FileExists(GetDatabaseFileName())) 
                    CreateDatabaseAndTables(session, GetDatabaseFileName());            
        }
        
        string GetDatabaseFileName()
        {
            return DatabaseName;
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

                Esent.AddColumn(session, tableId, ChangeStoreTable.ChangeRootIdColumn, JET_coltyp.Text, JET_CP.Unicode);
                Esent.AddColumn(session, tableId, ChangeStoreTable.SequenceColumn, JET_coltyp.Long, JET_CP.None, ColumndefGrbit.ColumnAutoincrement);
                Esent.AddColumn(session, tableId, ChangeStoreTable.BodyColumn, JET_coltyp.LongText, JET_CP.Unicode);
                
                string keyDescription = string.Format("+{0}\0+{1}\0\0", ChangeStoreTable.ChangeRootIdColumn, ChangeStoreTable.SequenceColumn);
                Esent.CreateIndex(session, tableId, ChangeStoreTable.Index, keyDescription);
                
                Esent.CloseTable(session, tableId);

                transaction.Commit(CommitTransactionGrbit.None);
            }
        }

        public void StoreChange(string changeRootId, Change change)
        {
            using (var session = new Session(this.instance))
            {
                int sequence = StoreChange(session, changeRootId, change);

                if (!(change is CheckPointChange)) return;
                
                DeleteBelowSequence(session, changeRootId, sequence);
            }
        }

        int StoreChange(Session session, string changeRootId, Change change)
        {
            JET_DBID dbId = Esent.OpenDatabase(session, GetDatabaseFileName());

            using (var table = new Table(session, dbId, ChangeStoreTable.Name, OpenTableGrbit.None))
            {
                var columns = Esent.GetColumns(session, table);

                using (var transaction = new Transaction(session))
                {
                    int autoIncrement = StoreChange(session, table, columns, changeRootId, change);
                    transaction.Commit(CommitTransactionGrbit.None);

                    return autoIncrement;
                }
            }
        }
        
        int StoreChange(Session session, Table table, IDictionary<string, JET_COLUMNID> columns, string changeRootId, Change change)
        {
            using (var update = new Update(session, table, JET_prep.Insert))
            {
                Esent.SetColumn(session, table, columns[ChangeStoreTable.ChangeRootIdColumn], changeRootId, Encoding.Unicode);
                Esent.SetColumn(session, table, columns[ChangeStoreTable.BodyColumn], this.serialiser.Serialise(change));
                int autoIncrement = Esent.RetrieveAutoIncrementColumn(session, table, columns[ChangeStoreTable.SequenceColumn]);
                
                update.Save();

                return autoIncrement;
            }
        }

        void DeleteBelowSequence(Session session, string changeRootId, int sequence)
        {
            JET_DBID dbId = Esent.OpenDatabase(session, GetDatabaseFileName());

            using (var table = new Table(session, dbId, ChangeStoreTable.Name, OpenTableGrbit.None))
                DeleteBelowSequence(session, table, changeRootId, sequence);             
        }

        void DeleteBelowSequence(Session session, Table table, string changeRootId, int sequence)
        {
            IDictionary<string, JET_COLUMNID> columns = Esent.GetColumns(session, table);

            Esent.UseIndex(session, table, ChangeStoreTable.Index);
            Esent.SetFirstSearchKey(session, table, changeRootId, Encoding.Unicode);
            Esent.SetSearchKey(session, table, 0);
            
            if (!Esent.TrySearchForGreaterThanKey(session, table)) return;

            Esent.SetFirstSearchKey(session, table, changeRootId, Encoding.Unicode);
            Esent.SetSearchKey(session, table, sequence);

            if (!Esent.TrySetIndexRange(session, table, SetIndexRangeGrbit.RangeUpperLimit)) return;

            while (Esent.TryMoveNext(session, table)) 
                Api.JetDelete(session, table);
        }

        public IEnumerable<Change> GetChanges(string changeRootId)
        {                    
            using (var session = new Session(this.instance))
            {
                JET_DBID dbId = Esent.OpenDatabase(session, GetDatabaseFileName());

                using (var table = new Table(session, dbId, ChangeStoreTable.Name, OpenTableGrbit.None))
                return GetChanges(changeRootId, session, table);                
            }
        }

        IEnumerable<Change> GetChanges(string changeRootId, Session session, Table table)
        {
            var changes = new List<Change>();
            IDictionary<string, JET_COLUMNID> columns = Esent.GetColumns(session, table);

            Esent.UseIndex(session, table, ChangeStoreTable.Index);
            
            Esent.SetFirstSearchKey(session, table, changeRootId, Encoding.Unicode);
            Esent.SetSearchKey(session, table, 0);
            
            if (!Esent.TrySearchForGreaterThanKey(session, table)) return changes;

            Esent.SetFirstSearchKey(session, table, changeRootId, Encoding.Unicode);
            Esent.SetSearchKey(session, table, int.MaxValue);
            
            if (!Esent.TrySetIndexRange(session, table, SetIndexRangeGrbit.RangeUpperLimit)) return changes;

            while (Esent.TryMoveNext(session, table))
                changes.Add(GetChangeFromColumn(session, table, columns[ChangeStoreTable.BodyColumn]));                
            
             return changes;
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