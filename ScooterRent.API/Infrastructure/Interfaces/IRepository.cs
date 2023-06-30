using MongoDB.Bson;
using MongoDB.Driver;
using ScooterRent.API.Entities;
using System.Linq.Expressions;

namespace ScooterRent.API.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Add(T entity);
        List<T> GetList(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        void Remove(string id);
        T Update(T entity);
    }
}
