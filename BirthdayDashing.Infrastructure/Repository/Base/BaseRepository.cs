using BirthdayDashing.Infrastructure.Data.Write;
using System.Data;

namespace BirthdayDashing.Infrastructure.Repository.Base
{
    public abstract class BaseRepository
    {
        private IDbSet DbSet;
        protected IDbConnection DbEntities => DbSet.GetDbEntities();
        protected IDbTransaction Transaction => DbSet.GetTransactionInstance();
        public BaseRepository(IDbSet dbSet)
        {
            DbSet = dbSet;
        }
    }
}
