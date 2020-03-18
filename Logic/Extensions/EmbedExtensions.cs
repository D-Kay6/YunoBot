namespace Logic.Extensions
{
    using Discord;

    public static class EmbedExtensions
    {
        public static Embed CreateEmbed(string title, string message)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithDescription(message);
            return embed.Build();
        }

        public static Embed CreateEmbed(string title, string message, Color color)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithDescription(message);
            embed.WithColor(color);
            return embed.Build();
        }

        public static Embed CreateEmbed(string title, string message, Color color, string image)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithDescription(message);
            embed.WithColor(color);
            embed.WithImageUrl(image);
            return embed.Build();
        }
    }
}