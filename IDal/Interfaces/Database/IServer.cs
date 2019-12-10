using Entity;

namespace IDal.Interfaces.Database
{
    public interface IServer
    {
        void AddServer(ulong id, string name);
        void AddServer(Server server);
        void UpdateServer(ulong id, string name);
        void UpdateServer(Server server);
        void DeleteServer(ulong id);
        Server GetServer(ulong id);
        void Save();
    }
}
