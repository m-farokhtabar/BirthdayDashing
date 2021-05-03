using System.Data;

namespace BirthdayDashing.Infrastructure.Data.Write
{
    public interface IDbSet
    {
        IDbConnection GetDbEntities();
        IDbTransaction GetTransactionInstance();
    }
}
