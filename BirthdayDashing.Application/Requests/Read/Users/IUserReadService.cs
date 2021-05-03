using BirthdayDashing.Application.Dtos.Users.Input;
using BirthdayDashing.Application.Dtos.Users.Output;
using System;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Requests.Read.Users
{
    public interface IUserReadService
    {
        Task<UserDto> Get(Guid id);
        Task<UserWithRolesNameDto> GetAuthentocateDataAsync(LoginDto login);
    }
}