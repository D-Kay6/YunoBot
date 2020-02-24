using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Mixture
{
    public class LanguageRepository : IDbLanguage
    {
        public async Task<bool> SetLanguage(ulong serverId, Language language)
        {
            await using var context = new DataContext();
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
            await using var context = new DataContext();
            var settings = await context.LanguageSettings.FindAsync(serverId);
            return settings.Language;
        }
    }
}