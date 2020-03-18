namespace IDal
{
    using System.Threading.Tasks;
    using Core.Entity;

    public interface ILocalization
    {
        Task<Localization> Read(string language);
    }
}