namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}
