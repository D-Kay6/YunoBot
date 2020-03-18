namespace IDal.Database
{
    using System.Threading.Tasks;
    using Core.Entity;

    public interface IDbAutoRole
    {
        Task Add(AutoRole value);
        Task Update(AutoRole value);
        Task Remove(AutoRole value);
        Task<AutoRole> Get(ulong serverId);
    }
}