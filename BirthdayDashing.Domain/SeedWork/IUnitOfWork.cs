namespace BirthdayDashing.Domain.SeedWork
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        void RollBack();
    }
}