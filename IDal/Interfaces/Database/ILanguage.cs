using Entity;

namespace IDal.Interfaces.Database
{
    public interface ILanguage
    {
        bool SetLanguage(ulong serverId, Language language);
        Language GetLanguage(ulong serverId);
    }
}
