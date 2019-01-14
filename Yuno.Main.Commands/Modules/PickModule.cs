using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Yuno.Main.Extentions;

namespace Yuno.Main.Commands.Modules
{
    public class PickModule : ModuleBase<SocketCommandContext>
    {
        private Random _random = new Random();

        [Command("pick")]
        public async Task PickCommand([Remainder]string message)
        {
            var selection = GetRandomOption(message);
            var embed = EmbedExtentions.CreateEmbed($"Choice for {Context.User.Username}", selection, new Color(255, 255, 0));

            await ReplyAsync("", false, embed);
        }

        private string GetRandomOption(string message)
        {
            var options = message.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            return options[_random.Next(options.Length)];
        }
    }
}
