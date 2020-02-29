using Core.Entity;
using System.Threading.Tasks;

namespace IDal
{
    public interface ILocalization
    {
        Task<Localization> Read(string language);
    }
}