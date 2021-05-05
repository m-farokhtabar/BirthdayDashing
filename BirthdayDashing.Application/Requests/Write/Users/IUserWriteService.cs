using BirthdayDashing.Application.Dtos.Users.Input;
using System;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Requests.Write.Users
{
    public interface IUserWriteService
    {
        Task AddAsync(AddUserDto user);
        Task UpdateAsync(Guid Id, UpdateUserDto user);
        Task ConfirmByEmailAsync(ConfirmUserDto confirmUser);
        Task ChangePasswordAsync(Guid id, ChangePasswordDto password);
    }
}