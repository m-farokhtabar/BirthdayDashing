using System.Data;

namespace BirthdayDashing.Application.Data
{
    public interface IReadDbSet
    {
        IDbConnection GetDbEntities();
    }
}
