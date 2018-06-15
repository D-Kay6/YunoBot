namespace AreannaBot
{
    internal class Program
    {
        static void Main(string[] args) => new AreannaBot().StartAsync().GetAwaiter().GetResult(); // Start the bot.
    }
}
