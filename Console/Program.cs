using System.Threading.Tasks;
using ILogic;
using LogicFactory;

namespace Yuno
{
    class Program
    {
        private static IBot _bot = BotFactory.GenerateBot();

        private static async Task Main(string[] args)
        {
            await _bot.Start();
        }
    }
}
