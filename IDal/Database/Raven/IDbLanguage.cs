namespace IDal.Database.Raven
{
    using System.Threading.Tasks;
    using Entity.RavenDB;

    public interface IDbLanguage
    {
        Task Add(LanguageSetting value);
        Task Update(LanguageSetting value);
        Task Delete(string id);
        Task<LanguageSetting> Get(ulong serverId);
    }
}