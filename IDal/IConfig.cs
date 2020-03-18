namespace IDal
{
    using System.Threading.Tasks;
    using Core.Entity;

    public interface IConfig
    {
        Task<Configuration> Read();
        Task Write(Configuration config);
    }
}