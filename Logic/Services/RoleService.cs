using Core.Entity;
using IDal.Database;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class RoleService
    {
        private readonly IDbRole _dbRole;

        public RoleService(IDbRole dbRole)
        {
            _dbRole = dbRole;
        }

        public Task Update(Role role)
        {
            return _dbRole.Update(role);
        }
    }
}