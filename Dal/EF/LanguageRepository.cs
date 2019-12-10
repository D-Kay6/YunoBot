using Entity;
using IDal.Interfaces.Database;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF
{
    public class LanguageRepository : ILanguage
    {
        private DataContext _context;

        public LanguageRepository()
        {
            _context = new DataContext();
        }

        public bool SetLanguage(ulong serverId, Language language)
        {
            var settings = _context.LanguageSettings.Find(serverId);
            if (settings == null) return false;
            try
            {
                settings.Language = language;
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public Language GetLanguage(ulong serverId)
        {
            return _context.LanguageSettings.Find(serverId).Language;
        }
    }
}
