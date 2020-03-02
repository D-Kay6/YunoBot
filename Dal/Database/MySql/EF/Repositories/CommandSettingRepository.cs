using Core.Entity;
using IDal.Database;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class CommandSettingRepository : IDbCommandSetting
    {
        public async Task Add(CommandSetting value)
        {
            await using var context = new DataContext();
            context.CommandSettings.Add(value);
            await context.SaveChangesAsync();
        }

        public async Task Update(CommandSetting value)
        {
            await using var context = new DataContext();
            context.CommandSettings.Update(value);
            await context.SaveChangesAsync();
        }

        public async Task Remove(CommandSetting value)
        {
            await using var context = new DataContext();
            context.CommandSettings.Remove(value);
            await context.SaveChangesAsync();
        }

        public async Task<CommandSetting> Get(ulong serverId)
        {
            await using var context = new DataContext();
            return await context.CommandSettings.FindAsync(serverId);
        }
    }
}