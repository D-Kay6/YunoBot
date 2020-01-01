using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.EF
{
    public class LanguageRepository : IDbLanguage
    {
        private readonly DataContext _context;

        public LanguageRepository()
        {
            _context = new DataContext();
        }

        public async Task<bool> SetLanguage(ulong serverId, Language language)
        {
            var settings = await _context.LanguageSettings.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.Language = language;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<Language> GetLanguage(ulong serverId)
        {
            var settings = await _context.LanguageSettings.FindAsync(serverId);
            return settings.Language;
        }
    }
}
