using System.Threading.Tasks;
using Entity;

namespace IDal.Interfaces.Database
{
    public interface IDbLanguage
    {
        Task<bool> SetLanguage(ulong serverId, Language language);
        Task<Language> GetLanguage(ulong serverId);
    }
}
