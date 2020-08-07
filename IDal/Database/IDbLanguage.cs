using Core.Enum;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbLanguage
    {
        Task<bool> SetLanguage(ulong serverId, Language language);
        Task<Language> GetLanguage(ulong serverId);
    }
}