using System;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Roles
{
    public interface IRoleReadService
    {
        Task<Guid?> GetIdByNameAsync(string name);
    }
}