using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;

namespace IDal.Database
{
    public interface IDbCommand
    {
        Task<string> GetPrefix(ulong serverId);
        Task<bool> SetPrefix(ulong serverId, string prefix);

        Task<bool> AddCustomCommand(ulong serverId, string command, string response);
        Task<bool> AddCustomCommand(CustomCommand customCommand);
        Task<bool> RemoveCustomCommand(ulong serverId, string command);
        Task<bool> RemoveCustomCommand(CustomCommand customCommand);
        Task<CustomCommand> GetCustomCommand(ulong serverId, string command);
        Task<List<CustomCommand>> GetCustomCommands(ulong serverId);
    }
}