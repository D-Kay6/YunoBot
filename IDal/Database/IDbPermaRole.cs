namespace IDal.Database
{
    using System.Threading.Tasks;
    using Core.Entity;

    public interface IDbPermaRole
    {
        Task Add(PermaRole value);
        Task Update(PermaRole value);
        Task Remove(PermaRole value);
        Task<PermaRole> Get(ulong serverId);
    }
}