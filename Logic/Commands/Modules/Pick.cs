using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Logic.Extentions;

namespace Logic.Commands.Modules
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
