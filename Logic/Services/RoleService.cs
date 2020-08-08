using Core.Entity;
using Discord;
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

        public Task Update(IRole role)
        {
            var data = new Role
            {
                Id = role.Id,
                Name = role.Name
            };
            return _dbRole.Update(data);
        }
    }
}