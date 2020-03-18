namespace Dal.Database.MySql.EF.Repositories
{
    using System.Threading.Tasks;
    using Core.Entity;
    using IDal.Database;
    using Microsoft.EntityFrameworkCore;

    public class CommandSettingRepository : BaseRepository, IDbCommandSetting
    {
        public async Task Add(CommandSetting value)
        {
            Context.CommandSettings.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(CommandSetting value)
        {
            Context.CommandSettings.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(CommandSetting value)
        {
            Context.CommandSettings.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<CommandSetting> Get(ulong serverId)
        {
            return await Context.CommandSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServerId == serverId);
        }
    }
}