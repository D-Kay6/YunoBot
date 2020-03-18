namespace IDal.Database.Raven
{
    using System.Threading.Tasks;
    using Entity.RavenDB;

    public interface IDbRole
    {
        Task Add(RoleAutomation value);
        Task Update(RoleAutomation value);
        Task Remove(string id);
        Task<RoleAutomation> Get(ulong serverId);
    }
}