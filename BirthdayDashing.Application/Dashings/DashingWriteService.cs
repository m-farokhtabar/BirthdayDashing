using BirthdayDashing.Application.Authorization;
using BirthdayDashing.Application.Dtos.Dashings.Input;
using BirthdayDashing.Domain.Dashing;
using BirthdayDashing.Domain.SeedWork;
using Common.Exception;
using System;
using System.Threading.Tasks;
using static Common.Exception.Messages;

namespace BirthdayDashing.Application.Dashings
{
    public class DashingWriteService : IDashingWriteService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IDashingRepository Repository;
        private readonly IAuthorizationService AuthorizationService;

        public DashingWriteService(IUnitOfWork unitOfWork, IDashingRepository repository, IAuthorizationService authorizationService)
        {
            UnitOfWork = unitOfWork;
            Repository = repository;
            AuthorizationService = authorizationService;
        }

        public async Task AddAsync(AddDashingDto dashing)
        {
            AuthorizationService.Authorized(dashing.UserId);

            Dashing entity = new(dashing.UserId, dashing.Birthday, dashing.Title, dashing.PostalCode, AuthorizationService.UserId, dashing.BackgroundUUID, dashing.Name, dashing.CurrentYearBirthday, dashing.City, dashing.State);
            try
            {
                await Repository.AddAsync(entity);
                UnitOfWork.SaveChanges();
            }
            catch
            {
                UnitOfWork.RollBack();
                throw;
            }
        }
        public async Task UpdateAsync(Guid id, UpdateDashingDto dashing)
        {            
            Dashing entity = await Repository.GetAsync(id);
            if (entity is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "Dashing"), ExceptionType.NotFound, nameof(id));
            
            AuthorizationService.Authorized(entity.UserId);

            entity.Update(dashing.Title, dashing.PostalCode, AuthorizationService.UserId, dashing.BackgroundUUID, dashing.Name, dashing.CurrentYearBirthday, dashing.City, dashing.State);
            try
            {
                await Repository.UpdateAsync(entity);
                UnitOfWork.SaveChanges();
            }
            catch
            {
                UnitOfWork.RollBack();
                throw;
            }
        }
        public async Task<bool> ToggleActiveAsync(Guid id)
        {
            Dashing entity = await Repository.GetAsync(id);
            if (entity is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "Dashing"), ExceptionType.NotFound, nameof(id));

            AuthorizationService.Authorized(entity.UserId);

            entity.ToggleActive(AuthorizationService.UserId);
            try
            {
                await Repository.UpdateActiveAsync(entity);
                UnitOfWork.SaveChanges();
            }
            catch
            {
                UnitOfWork.RollBack();
                throw;
            }
            return entity.Active;
        }
        public async Task<bool> ToggleDeletedAsync(Guid id)
        {
            Dashing entity = await Repository.GetAsync(id);
            if (entity is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "Dashing"), ExceptionType.NotFound, nameof(id));

            AuthorizationService.Authorized(entity.UserId);

            entity.ToggleDeleted(AuthorizationService.UserId);
            try
            {
                await Repository.UpdateDeletedAsync(entity);
                UnitOfWork.SaveChanges();
            }
            catch
            {
                UnitOfWork.RollBack();
                throw;
            }
            return entity.Deleted;
        }
    }
}
