using System.Threading.Tasks;

namespace IDal
{
    public interface ILogs
    {
        Task Write(string file, string data);
    }
}