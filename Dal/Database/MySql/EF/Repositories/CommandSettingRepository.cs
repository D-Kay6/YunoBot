using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class CommandSettingRepository : BaseRepository, IDbCommandSetting
    {
        public async Task Add(CommandSetting value)
        {
            Context.CommandSettings.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(CommandSetting value)
        {
            //try
            //{
            //    var entry = Context.Attach(value);
            //    entry.State = EntityState.Modified;
            //}
            //catch
            //{
            //    //ignore
            //}

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
            var value = await Context.CommandSettings.FindAsync(serverId);
            if (value != null)
                await Context.Entry(value).ReloadAsync();

            return value;
            //return await Context.CommandSettings
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.ServerId == serverId);
        }
    }
}