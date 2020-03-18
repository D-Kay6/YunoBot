namespace IDal
{
    using System.Threading.Tasks;

    public interface ILogs
    {
        Task Write(string file, string data);
    }
}