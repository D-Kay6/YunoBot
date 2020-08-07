using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class WelcomeSettingsRepository : BaseRepository, IDbWelcome
    {
        public async Task Add(WelcomeMessage value)
        {
            Context.WelcomeMessages.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(WelcomeMessage value)
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

            Context.WelcomeMessages.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(WelcomeMessage value)
        {
            Context.WelcomeMessages.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<WelcomeMessage> Get(ulong serverId)
        {
            var value = await Context.WelcomeMessages.FindAsync(serverId);
            if (value != null)
                await Context.Entry(value).ReloadAsync();

            return value;
            //return await Context.WelcomeMessages.FindAsync(serverId);
        }
    }
}