using DNH.Storage.MongoDB.Attributes;
using DNH.Storage.MongoDB.Models;
using DNH.Storage.MongoDB.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNH.Storage.MongoDB
{
    public class MongoDbContext
    {
        private readonly MongoClient _mongoClient;
        private readonly string _mongoDbDatabaseName;
        private readonly string _collectionPrefix;
        private IMongoClient client { get; set; }
        private IMongoDatabase Database { get { return _mongoClient.GetDatabase(_mongoDbDatabaseName); } }
        public MongoDbContext(MongDBSettings dbSettings)
        {
            var mongoDbConnectionString = dbSettings.MongoConnectionString;
            _mongoClient = new MongoClient(mongoDbConnectionString);

            _mongoDbDatabaseName = GetDatabaseName(mongoDbConnectionString);
            _collectionPrefix = dbSettings.TablePrefix;
        }
        private string GetDatabaseName(string mongoDbConnectionString)
        {
            var databaseName = mongoDbConnectionString.Substring(mongoDbConnectionString.LastIndexOf("/", StringComparison.InvariantCultureIgnoreCase) + 1);
            if (databaseName.Contains("?"))
            {
                databaseName = databaseName.Substring(0, databaseName.IndexOf("?", StringComparison.InvariantCultureIgnoreCase));
            }
            return databaseName;
        }
        public bool HasConnection()
        {
            var dbList = client.ListDatabases().ToList();

            if (dbList.Count > 0)
            {
                return true;
            }
            return false;
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : MongoBaseEntity
        {
            var type = typeof(TEntity);
            var collection = GetCollectionGeneric<TEntity>(type);

            if (HasEntityGroup(type))
            {
                collection = collection.OfType<TEntity>();
            }

            return collection;
        }

        public IQueryable<TEntity> GetCollectionAsQueryable<TEntity>() where TEntity : MongoBaseEntity
        {
            var type = typeof(TEntity);
            var collection = GetCollectionGeneric<TEntity>(type);

            if (HasEntityGroup(type))
            {
                collection = collection.OfType<TEntity>();
            }

            return collection.AsQueryable();
        }

        private IMongoCollection<TEntity> GetCollectionGeneric<TEntity>(Type type = null) where TEntity : MongoBaseEntity
        {
            var entityType = type ?? typeof(TEntity);
            var collectionName = GetCollectionName(entityType);

            return Database.GetCollection<TEntity>(collectionName);
        }

        private bool HasEntityGroup(Type type)
        {
            var attribute = type.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(EntityCollectionGroup));

            return attribute != null;
        }

        private string GetCollectionName(Type type)
        {
            var attribute = type.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(EntityCollectionGroup));

            return attribute != null
                ? attribute.ConstructorArguments.FirstOrDefault().Value.ToString()
                : type.Name;
        }
    }
}
