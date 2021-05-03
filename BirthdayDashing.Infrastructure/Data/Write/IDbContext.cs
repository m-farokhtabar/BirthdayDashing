using System.Data;

namespace BirthdayDashing.Infrastructure.Data.Write
{
    public interface IDbContext
    {
        IDbConnection GetDbEntities();
        IDbTransaction GetTransactionInstance();
        void DisposeConnection();
    }
}