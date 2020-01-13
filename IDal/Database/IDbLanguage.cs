using System.Threading.Tasks;
using Entity;

namespace IDal.Database
{
    public interface IDbLanguage
    {
        Task<bool> SetLanguage(ulong serverId, Language language);
        Task<Language> GetLanguage(ulong serverId);
    }
}
