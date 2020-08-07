using Core.Entity;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbWelcome
    {
        Task Add(WelcomeMessage value);
        Task Update(WelcomeMessage value);
        Task Remove(WelcomeMessage value);
        Task<WelcomeMessage> Get(ulong serverId);
    }
}