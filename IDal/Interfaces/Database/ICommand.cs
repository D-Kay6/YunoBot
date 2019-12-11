namespace IDal.Interfaces.Database
{
    public interface ICommand
    {
        string GetPrefix(ulong serverId);
        bool SetPrefix(ulong serverId, string prefix);
    }
}