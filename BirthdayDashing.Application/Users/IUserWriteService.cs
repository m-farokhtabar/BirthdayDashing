using BirthdayDashing.Application.Dtos.Users.Input;
using BirthdayDashing.Application.Dtos.Users.Output;
using System;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Users
{
    public interface IUserWriteService
    {
        Task AddAsync(AddUserDto user);
        Task UpdateAsync(Guid Id, UpdateUserDto user);
        Task ConfirmByEmailAsync(ConfirmUserByEmailDto confirmUser);
        Task ChangePasswordAsync(Guid id, ChangePasswordDto password);
        Task ResetPasswordAsync(Guid id, ResetPasswordDto password);
        Task<UserLoginDto> Login(LoginDto login);
    }
}