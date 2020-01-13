using System.Threading.Tasks;

namespace IDal
{
    public interface ILocalization
    {
        Task<Entity.Localization> Read(string language);
    }
}