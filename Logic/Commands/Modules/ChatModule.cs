using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using IDal.Database;
using Logic.Exceptions;
using Logic.Models.Chat;
using Logic.Services;
using System;
using System.Threading.Tasks;
using CommandService = Logic.Services.CommandService;

namespace Logic.Commands.Modules
{
    [Group("chat")]
    public class ChatModule : InteractiveBase<SocketCommandContext>
    {
        private readonly ChatService _chat;
        private readonly CommandService _command;
        private readonly LogsService _logs;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public ChatModule(ChatService chat, CommandService command, LogsService logs, IDbLanguage language, LocalizationService localization)
        {
            _chat = chat;
            _command = command;
            _logs = logs;
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
        public async Task DefaultChat()
        {
            var commandPrefix = await _command.GetPrefix(Context.Guild.Id);
            ChatSession session;
            try
            {
                session = await _chat.CreateSession(Context.User as SocketGuildUser, Context.Channel);
            }
            catch (SessionExistsException e)
            {
                await ReplyAsync(_localization.GetMessage("Chat error exists", commandPrefix));
                await _logs.Write("Chat", e, Context.Guild);
                return;
            }

            await ReplyAsync(_localization.GetMessage("Chat start"));
            while (!session.HasEnded)
            {
                var userMessage = await Interactive.NextMessageAsync(Context, timeout: TimeSpan.FromMinutes(1));
                if (userMessage == null)
                {
                    if (session.HasEnded)
                    {
                        break;
                    }
                    await ReplyAsync(_localization.GetMessage("Chat end timeout"));
                    return;
                }

                if (userMessage.Content.StartsWith(commandPrefix, StringComparison.OrdinalIgnoreCase) || userMessage.Content.StartsWith(Context.Client.CurrentUser.Mention, StringComparison.OrdinalIgnoreCase))
                {
                    if (userMessage.Content.EndsWith("chat stop", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                    continue;
                }

                var response = await session.GetResponse(userMessage.Content);
                await ReplyAsync(response);
            }

            await ReplyAsync(_localization.GetMessage("Chat end"));
        }

        [Command("stop")]
        public async Task ChatStop()
        {
            try
            {
                await _chat.EndSession(Context.User as SocketGuildUser);
            }
            catch (InvalidSessionException e)
            {
                await ReplyAsync(_localization.GetMessage("Chat error none"));
            }
        }
    }
}