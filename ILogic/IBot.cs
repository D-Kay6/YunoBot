namespace ILogic
{
    using System.Threading.Tasks;

    public interface IBot
    {
        Task Start();
        Task Stop();
    }
}