namespace Yuno
{
    using System;
    using System.Threading.Tasks;
    using ILogic;
    using LogicFactory;

    internal class Program
    {
        private static readonly IBot Bot = BotFactory.GenerateBot();

        private static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += OnExit;

            Console.Title = "Yuno Discord Bot";
            await Bot.Start();
        }

        private static async void OnExit(object sender, EventArgs e)
        {
            await Bot.Stop();
        }
    }
}