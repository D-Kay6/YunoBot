namespace Dal.Database.MySql.EF.Repositories
{
    public abstract class BaseRepository
    {
        protected BaseRepository()
        {
            Context = new DataContext();
        }

        protected DataContext Context { get; }
    }
}