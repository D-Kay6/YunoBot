using System;
using ILogic;
using LogicFactory;
using System.Threading.Tasks;

namespace Yuno
{
    internal class Program
    {
        private static readonly IBot Bot = BotFactory.GenerateBot();

        private static async Task Main(string[] args)
        {
            Console.Title = "Yuno Discord Bot";
            await Bot.Start();
        }
    }
}
