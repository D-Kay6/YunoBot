namespace Dal.Database.RavenDB.Repositories
{
    public abstract class BaseRepository
    {
        private static RavenContext _context;

        internal RavenContext Context
        {
            get { return _context ??= new RavenContext(); }
        }
    }
}