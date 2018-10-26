using Yuno.Main.Core;
using Yuno.Main.Factory;

namespace Yuno
{
    class Program
    {
        private static IBot _bot = BotFactory.GenerateBot();
        static void Main(string[] args) => _bot.Start().GetAwaiter().GetResult();
    }
}
