using IDal.Structs.Database;

namespace IDal.Interfaces.Database
{
    public interface IAutoRole
    {
        bool IsAutoRole(ulong serverId, string name);
        bool IsPermaRole(ulong serverId, string name);

        bool SetAutoPrefix(ulong serverId, string prefix);
        bool SetPermaPrefix(ulong serverId, string prefix);

        RoleData GetData(ulong serverId);

        bool IsRoleIgnore(ulong serverId, ulong playerId);
        bool AddRoleIgnore(ulong serverId, ulong playerId);
        bool RemoveRoleIgnore(ulong serverId, ulong playerId);
    }
}