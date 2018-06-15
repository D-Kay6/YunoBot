using Factory;

namespace AreannaBot
{
    internal class Program
    {
        static void Main(string[] args) => ConnectionFactory.GenerateConnection().Start().GetAwaiter().GetResult(); // Start the bot.
    }
}
