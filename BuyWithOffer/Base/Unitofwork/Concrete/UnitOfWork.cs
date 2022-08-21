using System;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserDbContext dbContext;
        public bool disposed;

        public UnitOfWork(UserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task CompleteAsync()
        {
            await dbContext.SaveChangesAsync();
        }
        protected virtual void Clean(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Clean(true);
            GC.SuppressFinalize(this);
        }
    }
}
