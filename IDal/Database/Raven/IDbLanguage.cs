using Entity.RavenDB;
using System.Threading.Tasks;

namespace IDal.Database.Raven
{
    public interface IDbLanguage
    {
        Task Add(LanguageSetting value);
        Task Update(LanguageSetting value);
        Task Delete(string id);
        Task<LanguageSetting> Get(ulong serverId);
    }
}