using System.Threading.Tasks;
using Entity;

namespace IDal
{
    public interface IConfig
    {
        Task<Configuration> Read();
        Task Write(Configuration config);
    }
}
