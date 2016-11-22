using System.Data.Entity;
using Superheroes.Repositories.Contracts;

namespace Superheroes.Repositories
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;

        public EfUnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Commit()
        {
            this.dbContext.SaveChanges();
        }
        public void Dispose()
        {
        }
    }
}
