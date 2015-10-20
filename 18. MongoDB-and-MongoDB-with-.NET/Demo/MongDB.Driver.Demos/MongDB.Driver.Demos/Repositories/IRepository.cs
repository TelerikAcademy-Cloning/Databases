using MongDB.Driver.Demos.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MongDB.Driver.Demos.Repositories
{
    public interface IRepository<T>
        where T : IEntity
    {
        Task Add(T value);

        Task<IQueryable<T>> All();

        Task Delete(object id);

        Task Delete(T obj);
    }
}