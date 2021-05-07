using BirthdayDashing.Domain.SeedWork;

namespace BirthdayDashing.Infrastructure.Data.Write
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext DbContext;        
        public UnitOfWork(IDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public void SaveChanges()
        {
            try
            {
                DbContext.GetTransactionInstance().Commit();
            }
            catch
            {
                RollBackTrans();
                throw;
            }
            finally
            {
                DbContext.DisposeConnection();
            }
        }
        public void RollBack()
        {
            RollBackTrans();
            DbContext.DisposeConnection();
        }
        private void RollBackTrans()
        {
            try
            {
                DbContext.GetTransactionInstance().Rollback();
            }
            catch
            {
                throw;
            }
        }
    }
}
