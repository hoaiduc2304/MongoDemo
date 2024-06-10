using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DNH.Storage.MongoDB.Models;
using MongoDB.Driver;

namespace DNH.Storage.MongoDB.Repository
{
    public interface IMongoRepository<T> where T : MongoBaseEntity
    {
        Task<T> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<T> GetAsync(CancellationToken cancellationToken, params object[] keyValues);

        Task AddAsync(T entity, CancellationToken cancellationToken);

        Task UpdateAsync(string id, T entity, CancellationToken cancellationToken);

        Task RemoveAsync(CancellationToken cancellationToken, params object[] keyValues);

        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        Task<T> GetOneAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        Task<IEnumerable<T>> GetAllDynamicAsync(string propertyName, object value, CancellationToken cancellationToken);

        Task<long> CountAsync(CancellationToken cancellationToken);

        Task<long> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        Task<UpdateResult> UpdateOneAsync(string id, T entity, CancellationToken cancellationToken);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        Task<T> GetAsync(FilterDefinition<T> predicate, CancellationToken cancellationToken);

        Task<IEnumerable<T>> GetAllWithPagingAsync(Expression<Func<T, bool>> predicate, int? page, int? pageSize);

        Task Upsert(Expression<Func<T, bool>> predicate, T entity);
    }
}
