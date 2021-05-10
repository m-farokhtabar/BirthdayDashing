using BirthdayDashing.Application.Dtos.Dashings.Input;
using System;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Dashings
{
    public interface IDashingWriteService
    {
        Task AddAsync(AddDashingDto dashing);
        Task UpdateAsync(Guid id, UpdateDashingDto dashing);
        Task<bool> ToggleActiveAsync(Guid id);
        Task<bool> ToggleDeletedAsync(Guid id);
    }
}