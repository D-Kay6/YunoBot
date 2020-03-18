namespace Logic.Modules.Moderation
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Extensions;
    using IDal.Database;
    using Services;

    [RequireUserPermission(GuildPermission.ManageMessages)]
    [Group("Message")]
    public class MessageModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public MessageModule(IDbLanguage language, LocalizationService localization)
        {
            _language = language;
            _localization = localization;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            Task.WaitAll(Prepare());
            base.BeforeExecute(command);
        }

        private async Task Prepare()
        {
            await _localization.Load(await _language.GetLanguage(Context.Guild.Id));
        }

        [Group("Move")]
        public class MessageMoveModule : ModuleBase<SocketCommandContext>
        {
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;
            private readonly string[] ImageExtensions = {"jpg", "jpeg", "png", "gif"};

            public MessageMoveModule(IDbLanguage language, LocalizationService localization)
            {
                _language = language;
                _localization = localization;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                Task.WaitAll(Prepare());
                base.BeforeExecute(command);
            }

            private async Task Prepare()
            {
                await _localization.Load(await _language.GetLanguage(Context.Guild.Id));
            }

            [Command]
            public async Task DefaultMessageMove(SocketTextChannel channel, bool deleteOriginal = true)
            {
                await Context.Message.DeleteAsync();

                var message = await GetMessage();
                if (message == null)
                {
                    await ReplyAsync("I could not find the message. Are you sure there are any?");
                    return;
                }

                await MoveMessageEmbed(message, channel);

                if (deleteOriginal)
                    await message.DeleteAsync();
            }

            [Command]
            public async Task DefaultMessageMove(SocketTextChannel channel, ulong messageId, bool deleteOriginal = true)
            {
                await Context.Message.DeleteAsync();

                var message = await GetMessage(messageId);
                if (message == null)
                {
                    await ReplyAsync("I could not find the message. Are you sure you have the right ID?");
                    return;
                }

                await MoveMessageEmbed(message, channel);

                if (deleteOriginal)
                    await message.DeleteAsync();
            }

            [Command("Plain")]
            public async Task MessageMovePlain(SocketTextChannel channel, bool deleteOriginal = true)
            {
                await Context.Message.DeleteAsync();

                var message = await GetMessage();
                if (message == null)
                {
                    await ReplyAsync("I could not find the message. Are you sure there are any?");
                    return;
                }

                await MoveMessageText(message, channel);

                if (deleteOriginal)
                    await message.DeleteAsync();
            }

            [Command("Plain")]
            public async Task MessageMovePlain(SocketTextChannel channel, ulong messageId, bool deleteOriginal = true)
            {
                await Context.Message.DeleteAsync();

                var message = await GetMessage(messageId);
                if (message == null)
                {
                    await ReplyAsync("I could not find the message. Are you sure you have the right ID?");
                    return;
                }

                await MoveMessageText(message, channel);

                if (deleteOriginal)
                    await message.DeleteAsync();
            }

            private async Task<IMessage> GetMessage(ulong messageId = 0)
            {
                if (messageId > 0) return await Context.Channel.GetMessageAsync(messageId);

                var result = Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, 1);
                var messages = await result.Skip(1).FirstOrDefaultAsync();
                return messages.FirstOrDefault();
            }

            private async Task MoveMessageEmbed(IMessage message, SocketTextChannel channel)
            {
                var author = message.Author as SocketGuildUser;
                var attachment = message.Attachments.FirstOrDefault();

                var embedBuilder = new EmbedBuilder();
                embedBuilder.WithThumbnailUrl(author.GetAvatarUrl());
                embedBuilder.WithAuthor(author.Nickname());
                embedBuilder.WithTimestamp(message.Timestamp);
                embedBuilder.WithFooter(Context.Channel.Name);
                if (!string.IsNullOrEmpty(message.Content))
                    embedBuilder.WithDescription(message.Content);

                if (attachment != null)
                {
                    var extension = Path.GetExtension(attachment.Filename);
                    if (!ImageExtensions.Any(x => x.Equals(extension, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        embedBuilder.WithImageUrl(attachment.Url);
                        await channel.SendMessageAsync(embed: embedBuilder.Build());
                        return;
                    }
                }

                using var webClient = new WebClient();
                var file = await webClient.DownloadDataTaskAsync(new Uri(attachment.Url));
                await using var memoryStream = new MemoryStream(file);
                await channel.SendFileAsync(memoryStream, attachment.Filename, string.Empty,
                    embed: embedBuilder.Build());
            }

            private async Task MoveMessageText(IMessage message, SocketTextChannel channel)
            {
                var author = message.Author as SocketGuildUser;
                var attachment = message.Attachments.FirstOrDefault();

                var msg = string.Format("{0} - {1:HH:mm:ss yyyy/MM/dd}  •  {2}\n{3}",
                    author.Nickname(),
                    message.Timestamp.DateTime,
                    Context.Channel.Name,
                    message.Content);

                if (attachment == null)
                {
                    await channel.SendMessageAsync(msg);
                    return;
                }

                using var webClient = new WebClient();
                var file = await webClient.DownloadDataTaskAsync(new Uri(attachment.Url));
                await using var memoryStream = new MemoryStream(file);
                await channel.SendFileAsync(memoryStream, attachment.Filename, msg);
            }
        }
    }
}