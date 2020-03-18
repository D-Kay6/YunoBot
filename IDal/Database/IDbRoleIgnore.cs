namespace IDal.Database
{
    using System.Threading.Tasks;
    using Core.Entity;

    public interface IDbRoleIgnore
    {
        Task Add(RoleIgnore value);
        Task Update(RoleIgnore value);
        Task Remove(RoleIgnore value);
        Task<RoleIgnore> Get(ulong serverId, ulong userId);
    }
}