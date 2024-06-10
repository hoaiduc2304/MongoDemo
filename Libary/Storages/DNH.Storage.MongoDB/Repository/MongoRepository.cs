using DNH.Storage.MongoDB.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DNH.Storage.MongoDB.Exceptions;

namespace DNH.Storage.MongoDB.Repository
{
    public class MongoRepository<T> : IMongoRepository<T> where T : MongoBaseEntity
    {
        private readonly MongoDbContext _dc;
        protected readonly IMongoCollection<T> collection;
        protected readonly IQueryable<T> queryable;
        private readonly IServiceProvider _services;
        private UpdateOptions _options;
        public MongoRepository(MongoDbContext dc, IServiceProvider services)
        {
            _dc = dc;
            _services = services;
            collection = _dc.GetCollection<T>();
            queryable = _dc.GetCollectionAsQueryable<T>();
            _options = new UpdateOptions
            {
                IsUpsert = true,
            };
        }
        protected UpdateOptions updateOptions
        {
            get
            {
                return new UpdateOptions() { };
            }
        }
        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            var newObjectId = ObjectId.GenerateNewId();
            entity.Id = newObjectId.ToString();
            entity.CreatedAt = entity.UpdatedAt = DateTime.UtcNow;
            await collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }


        public virtual async Task<long> CountAsync(CancellationToken cancellationToken)
        {
            return await collection.CountAsync(x => true, cancellationToken: cancellationToken);
        }

        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await collection.CountAsync(predicate, cancellationToken: cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(x => true, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAllWithPagingAsync(Expression<Func<T, bool>> predicate, int? page, int? pageSize)
        {
            var query = collection.Find(predicate);

            if (page.HasValue && pageSize.HasValue)
            {
                query.Skip((page.Value - 1) * pageSize.Value).Limit(pageSize.Value);
            }

            return await query.ToListAsync();
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {

            var cursor = await collection.FindAsync(predicate, cancellationToken: cancellationToken);

            return cursor.ToList();
        }

        public virtual async Task<T> GetOneAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {

            var data = await collection.FindAsync(predicate, cancellationToken: cancellationToken);
            var result = await data.FirstOrDefaultAsync(cancellationToken: cancellationToken);
            return result;
        }

        public virtual async Task<IEnumerable<T>> GetAllDynamicAsync(string propertyName, object value, CancellationToken cancellationToken)
        {
            var cursor = await collection.FindAsync(Builders<T>.Filter.Eq(propertyName, value), cancellationToken: cancellationToken);

            return cursor.ToEnumerable();
        }
        public virtual async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken)
        {

            var data = await collection.FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
            var result = await data.FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return result;
        }

        public virtual async Task<T> GetAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            string keyValue = keyValues.FirstOrDefault()?.ToString();

            var data = await collection.FindAsync(x => x.Id == keyValue, cancellationToken: cancellationToken);
            var result = await data.FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return result;
        }

        public virtual async Task<T> GetAsync(FilterDefinition<T> predicate, CancellationToken cancellationToken)
        {
            var data = await collection.FindAsync(predicate, cancellationToken: cancellationToken);

            return await data.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            var data = await collection.FindAsync(predicate, cancellationToken: cancellationToken);

            return await data.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }



        public virtual async Task RemoveAsync(CancellationToken cancellationToken, params object[] keyValues)
        {

            await collection.DeleteOneAsync(x => x.Id == keyValues.FirstOrDefault().ToString(), cancellationToken: cancellationToken);
        }

        public virtual async Task UpdateAsync(string id, T entity, CancellationToken cancellationToken)
        {
            entity.UpdatedAt = DateTime.Now;
            await collection.ReplaceOneAsync(x => x.Id.Equals(id), entity, updateOptions, cancellationToken);
        }

        public virtual async Task<UpdateResult> UpdateOneAsync(string id, T entity, CancellationToken cancellationToken)
        {
            UpdateResult updateResult;
            // Get the current document
            var document = await collection.Find(x => x.Id == id).SingleAsync();

            // Update the document only if document version is the same
            var filter = Builders<T>.Filter;
            var update = Builders<T>.Update;
            var updates = new List<UpdateDefinition<T>>();

            var properties = typeof(T).GetProperties().Where(x => x.Name != "DocumentVersion" && x.Name != "Id" && x.Name != "UpdatedAt" && x.Name != "ProductGroups").ToList();

            properties.ForEach(x =>
            {
                var propName = x.Name;
                var updatedValue = x.GetValue(entity);
                if (updatedValue != null)
                {
                    var originalValue = x.GetValue(document);
                    if (!originalValue.Equals(updatedValue))
                    {
                        updates.Add(update.Set(propName, updatedValue));
                    }
                }

            });


            var updateFilter = filter.Eq(t => t.Id, id);
            updateResult = await collection.UpdateOneAsync(updateFilter, update.Combine(updates), updateOptions, cancellationToken);

            try
            {
                updateResult = await collection.UpdateOneAsync(updateFilter, update.Combine(updates), updateOptions, cancellationToken);
            }
            catch (Exception exception)
            {
                throw new LSException("Update Failed! please try again!", exception);
            }

            // if nothing was the modified after Update operator action
            if (updateResult.ModifiedCount == 0)
            {
                Exception exception = new Exception();
                throw new LSException("The document was updated from outside! Please fetch your data before change", exception);
            }


            return updateResult;
        }



        public async Task Upsert(Expression<Func<T, bool>> predicate, T entity)
        {
            await collection.ReplaceOneAsync(predicate, options: new UpdateOptions { IsUpsert = true }, replacement: entity);
        }
    }
}
