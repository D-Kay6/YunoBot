using System.Threading.Tasks;
using Entity.RavenDB;

namespace IDal.Database.Raven
{
    public interface IDbWelcome
    {
        Task Add(WelcomeMessage value);
        Task Update(WelcomeMessage value);
        Task Remove(string id);
        Task<WelcomeMessage> Get(ulong serverId);
    }
}