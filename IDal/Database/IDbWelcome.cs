namespace IDal.Database
{
    using System.Threading.Tasks;
    using Core.Entity;

    public interface IDbWelcome
    {
        Task Add(WelcomeMessage value);
        Task Update(WelcomeMessage value);
        Task Remove(WelcomeMessage value);
        Task<WelcomeMessage> Get(ulong serverId);
    }
}