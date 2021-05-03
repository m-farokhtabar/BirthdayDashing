namespace BirthdayDashing.Domain.Data
{
    public interface IUnitOfWork
    {
        void SaveChanges();
    }
}