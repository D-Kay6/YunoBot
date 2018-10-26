using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Yuno.Main.Extentions;

namespace Yuno.Main.Commands.Modules
{
    public class Pick : ModuleBase<SocketCommandContext>
    {
        [Command("pick")]
        public async Task Command([Remainder]string message)
        {
            var selection = GetRandomOption(message);
            var embed = EmbedExtention.CreateEmbed($"Choice for {Context.User.Username}", selection, new Color(255, 255, 0));

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        private string GetRandomOption(string message)
        {
            var options = message.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            return options[new Random().Next(options.Length)];
        }
    }
}
