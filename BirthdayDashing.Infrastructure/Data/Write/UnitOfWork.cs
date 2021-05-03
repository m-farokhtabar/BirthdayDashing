using BirthdayDashing.Domain.Data;
using System;

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
                DbContext.GetTransactionInstance().Rollback();
                throw new Exception("Data was not saved");
            }
            finally
            {
                DbContext.DisposeConnection();
            }
        }
    }
}
