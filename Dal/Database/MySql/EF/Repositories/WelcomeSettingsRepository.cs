namespace Dal.Database.MySql.EF.Repositories
{
    using System.Threading.Tasks;
    using Core.Entity;
    using IDal.Database;

    public class WelcomeSettingsRepository : BaseRepository, IDbWelcome
    {
        public async Task Add(WelcomeMessage value)
        {
            Context.WelcomeMessages.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(WelcomeMessage value)
        {
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
            return await Context.WelcomeMessages.FindAsync(serverId);
        }
    }
}