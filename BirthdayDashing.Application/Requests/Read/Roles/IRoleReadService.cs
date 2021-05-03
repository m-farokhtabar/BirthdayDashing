using System;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Requests.Read.Roles
{
    public interface IRoleReadService
    {
        Task<Guid?> GetIdByNameAsync(string name);
    }
}