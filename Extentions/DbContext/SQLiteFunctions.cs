using System;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Linq;

namespace CB.CSharp.Extentions
{
    public static class SQLiteFunctions
    {
        public static string GetFileName(this DbContext DbContext)
        {
            var FileName = "";
            if (DbContext.Database.Connection is SQLiteConnection)
            {
                var SQLiteConnection = DbContext.Database.Connection as SQLiteConnection;
                SQLiteConnection.Open();
                FileName = SQLiteConnection.FileName;
                SQLiteConnection.Close();
            }
            return FileName;
        }

        public static string GetTableName(this DbContext DbContext, Type type)
        {
            var metadata = ((IObjectContextAdapter)DbContext).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var entityType = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == type);

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                    .Single()
                    .EntitySetMappings
                    .Single(s => s.EntitySet == entitySet);

            // Find the storage entity set (table) that the entity is mapped
            var table = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .StoreEntitySet;

            // Return the table name from the storage entity set
            return (string)table.MetadataProperties["Table"].Value ?? table.Name;
        }

        public static void CreateFTS<TEntity>(this DbContext DbContext, DbSet<TEntity> EnitySet, string TableName) where TEntity : class
        {
            var EntityTableName = DbContext.GetTableName(typeof(TEntity));

            DbContext.Database.ExecuteSqlCommand($"CREATE VIRTUAL TABLE {TableName} USING fts4(content='{EntityTableName}')");
            DbContext.Database.ExecuteSqlCommand($"INSERT INTO {TableName}({TableName}) VALUES('rebuild')");
        }
    }
}
