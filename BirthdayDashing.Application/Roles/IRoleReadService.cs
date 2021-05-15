using BirthdayDashing.Application.Dtos.Roles.Output;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Roles
{
    public interface IRoleReadService
    {
        Task<Guid?> GetIdByNameAsync(string name);
        Task<List<RoleDto>> GetAllAsync();
    }
}