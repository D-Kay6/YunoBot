using Cleverbot.Net;
using Discord.Addons.Interactive;
using Discord.Commands;
using IDal.Database;
using Logic.Services;
using System;
using System.Threading.Tasks;
using CleverbotSession = Cleverbot.Net.CleverbotSession;

namespace Logic.Commands.Modules
{
    [Group("chat")]
    public class ChatModule : InteractiveBase<SocketCommandContext>
    {
        private readonly ConfigurationService _configuration;
        private readonly LogsService _logs;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public ChatModule(ConfigurationService configuration, LogsService logs, IDbLanguage language, LocalizationService localization)
        {
            _configuration = configuration;
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
            var api = await _configuration.GetChatBotApi();

            var session = new CleverbotSession(api);
            CleverbotResponse response = null;

            await ReplyAsync(_localization.GetMessage("Chat start"));
            while (true)
            {
                var userMessage = await Interactive.NextMessageAsync(Context, timeout: TimeSpan.FromMinutes(1));
                if (userMessage == null)
                    break;

                response = response != null
                    ? await response.RespondAsync(userMessage.Content)
                    : await session.GetResponseAsync(userMessage.Content);
                await ReplyAsync(response.Response);
            }

            await ReplyAsync(_localization.GetMessage("Chat end"));
        }
    }
}