﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using IDal.Interfaces.Database;
using Logic.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("Ban")]
    [RequireUserPermission(GuildPermission.BanMembers)]
    public class BanModule : ModuleBase<SocketCommandContext>
    {
        private IDbBan _ban;
        private IDbUser _user;
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public BanModule(IDbBan ban, IDbUser user, IDbLanguage language)
        {
            _ban = ban;
            _user = user;
            _language = language;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            Task.WaitAll(LoadLanguage());
            base.BeforeExecute(command);
        }

        private async Task LoadLanguage()
        {
            _lang = new Localization.Localization(await _language.GetLanguage(Context.Guild.Id));
        }


        [Command]
        public async Task DefaultBan()
        {
            await ReplyAsync(_lang.GetMessage("Ban default"));
        }

        [Priority(-1)]
        [Command]
        public async Task BanUser([Remainder] string name)
        {
            await ReplyAsync(_lang.GetMessage("Invalid user", name));
        }

        [Command]
        public async Task BanUser(SocketGuildUser user)
        {
            var builder = new EmbedBuilder();
            builder.AddField(_lang.GetMessage("Ban user"), user.Mention);
            var time = _lang.GetMessage("Ban duration forever");
            var reason = _lang.GetMessage("Ban reason none");

            builder.AddField(_lang.GetMessage("Ban duration"), time);
            builder.AddField(_lang.GetMessage("Ban reason"), reason);
            await ReplyAsync(null, false, builder.Build());
            await user.BanAsync();
        }

        [Command]
        public async Task BanUser(SocketGuildUser user, [Remainder] string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                await BanUser(user);
                return;
            }

            var builder = new EmbedBuilder();
            builder.AddField(_lang.GetMessage("Ban user"), user.Mention);
            var time = _lang.GetMessage("Ban duration forever");
            var reason = _lang.GetMessage("Ban reason none");

            var parts = message.Split(' ');
            var duration = parts[0].GetDuration();
            var endDate = duration > TimeSpan.Zero ? DateTime.Now + duration : (DateTime?)null;
            if (endDate != null)
            {
                time = endDate.Value.ToString();
            }

            message = string.Join(' ', parts.Skip(1));
            if (!string.IsNullOrEmpty(message))
            {
                reason = message;
            }

            builder.AddField(_lang.GetMessage("Ban duration"), time);
            builder.AddField(_lang.GetMessage("Ban reason"), reason);

            await ReplyAsync(null, false, builder.Build());
            await _user.UpdateUser(user.Id, user.Username);
            await _ban.AddBan(user.Id, user.Guild.Id, endDate, reason);
            await user.BanAsync(0, string.IsNullOrEmpty(message) ? null : reason);
        }
    }
}