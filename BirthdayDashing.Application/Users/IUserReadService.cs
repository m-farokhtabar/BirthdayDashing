using BirthdayDashing.Application.Dtos.Users.Output;
using System;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Users
{
    public interface IUserReadService
    {
        Task<UserDto> GetAsync(Guid id);
        Task<UserEssentialDataDto> GetEssentialDataAsync(Guid id);
        Task<UserEssentialDataDto> GetEssentialDataByEmailAsync(string email);
        Task<UserWithRolesNameDto> GetAuthenticationDataByEmailAsync(string email);
    }
}