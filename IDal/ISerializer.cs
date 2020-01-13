namespace IDal
{
    public interface ISerializer
    {
        T Read<T>(ulong id);
        void Write<T>(ulong id, T data);
    }
}
