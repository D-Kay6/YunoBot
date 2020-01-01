using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.EF
{
    public class LanguageRepository : IDbLanguage
    {
        public async Task<bool> SetLanguage(ulong serverId, Language language)
        {
            var context = new DataContext();
            var settings = await context.LanguageSettings.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.Language = language;
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<Language> GetLanguage(ulong serverId)
        {
            var context = new DataContext();
            var settings = await context.LanguageSettings.FindAsync(serverId);
            return settings.Language;
        }
    }
}
