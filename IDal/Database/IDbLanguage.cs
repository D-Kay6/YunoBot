namespace IDal.Database
{
    using System.Threading.Tasks;
    using Core.Enum;

    public interface IDbLanguage
    {
        Task<bool> SetLanguage(ulong serverId, Language language);
        Task<Language> GetLanguage(ulong serverId);
    }
}