using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using ScooterRent.API.Entities;
using ScooterRent.API.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace ScooterRent.API.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> collection;
        public Repository(IMongoDatabase database, string collectionName)
        {
            collection = database.GetCollection<T>(collectionName);
        }

        public T Add(T entity)
        {
            entity.CreatedDate = DateTime.Now;
            collection.InsertOne(entity);
            return entity;
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            return collection.Find(filter).FirstOrDefault();
        }

        public List<T> GetList(Expression<Func<T, bool>> filter = null)
        {
            return collection.Find(filter is null ? Builders<T>.Filter.Empty : filter).ToList();
        }

        public void Remove(string id)
        {
            collection.DeleteOne(x => x.Id == id);
        }

        public T Update(T entity)
        {
            collection.ReplaceOne(x => x.Id == entity.Id, entity);
            return entity;
        }
    }
}
