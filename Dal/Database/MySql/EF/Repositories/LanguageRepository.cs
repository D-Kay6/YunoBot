﻿using Core.Enum;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class LanguageRepository : BaseRepository, IDbLanguage
    {
        public async Task<bool> SetLanguage(ulong serverId, Language language)
        {
            var settings = await Context.LanguageSettings.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.Language = language;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<Language> GetLanguage(ulong serverId)
        {
            var settings = await Context.LanguageSettings.FindAsync(serverId);
            return settings.Language;
        }
    }
}