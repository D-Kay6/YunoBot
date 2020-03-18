namespace IDal.Database
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Entity;

    public interface IDbUser
    {
        Task AddUser(ulong id, string name);
        Task AddUser(User user);
        Task UpdateUser(ulong id, string name);
        Task UpdateUser(User user);
        Task<User> GetUser(ulong id);
        Task<List<User>> GetUsers();
    }
}