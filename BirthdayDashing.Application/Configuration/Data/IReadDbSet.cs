using System.Data;

namespace BirthdayDashing.Application.Configuration.Data
{
    public interface IReadDbSet
    {
        IDbConnection GetDbEntities();
    }
}
