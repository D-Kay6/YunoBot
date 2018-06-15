using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace Logic.Extentions
{
    public static class EmbedExtention
    {
        public static Embed CreateEmbed(string title, string message, Color color)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithDescription(message);
            embed.WithColor(color);
            return embed.Build();
        }
    }
}
