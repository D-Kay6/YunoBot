using Core.Entity;
using System.Threading.Tasks;

namespace IDal
{
    public interface IConfig
    {
        Task<Configuration> Read();
        Task Write(Configuration config);
    }
}