using Discord;
using Discord.Commands;
using Logic.Extentions;
using System;
using System.Threading.Tasks;

namespace Logic.Modules
{
    public class PickModule : ModuleBase<SocketCommandContext>
    {
        [Command("pick")]
        public async Task PickCommand([Remainder] string message)
        {
            var options = message.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            var embed = EmbedExtentions.CreateEmbed($"Choice for {Context.User.Username}", options.GetRandomItem(),
                new Color(255, 255, 0));

            await ReplyAsync("", false, embed);
        }
    }
}