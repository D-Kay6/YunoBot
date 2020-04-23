namespace IDal.Database
{
    using System.Threading.Tasks;
    using Core.Entity;

    public interface IDbUser
    {
        Task Add(User value);
        Task Update(User value);
        Task Remove(User value);
        Task<User> Get(ulong id);
    }
}