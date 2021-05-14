using BirthdayDashing.Application.Dtos.Dashings.Output;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Dashings
{
    public interface IDashingReadService
    {
        Task<DashingDto> GetAsync(Guid id);
        Task<List<DashingDto>> GetByUserIdAsync(Guid userId);
    }
}