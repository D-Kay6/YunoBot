using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using IDal.Interfaces.Database;

namespace Logic.Modules
{
    [Group("Kick")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public class KickModule : ModuleBase<SocketCommandContext>
    {
        private ILanguage _language;
        private Localization.Localization _lang;

        public KickModule(ILanguage language)
        {
            _language = language;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(_language.GetLanguage(Context.Guild.Id));
            base.BeforeExecute(command);
        }


        [Command]
        public async Task DefaultKick()
        {
            await ReplyAsync();
        }

        [Priority(-1)]
        [Command]
        public async void KickUser([Remainder] string name)
        {
            await ReplyAsync(_lang.GetMessage("Invalid user", name));
        }

        [Command]
        public async void KickUser(SocketGuildUser user, [Remainder] string reason = null)
        {
            await user.KickAsync(reason);
            await ReplyAsync(_lang.GetMessage("Kick performed", user.Username));
        }
    }

    [Group("Ban")]
    [RequireUserPermission(GuildPermission.BanMembers)]
    public class BanModule : ModuleBase<SocketCommandContext>
    {
        private ILanguage _language;
        private Localization.Localization _lang;

        public BanModule(ILanguage language)
        {
            _language = language;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(_language.GetLanguage(Context.Guild.Id));
            base.BeforeExecute(command);
        }


        [Command]
        public async Task DefaultBan()
        {
            await ReplyAsync(_lang.GetMessage("Ban default"));
        }

        [Priority(-1)]
        [Command]
        public async void KickUser([Remainder] string name)
        {
            await ReplyAsync(_lang.GetMessage("Invalid user", name));
        }

        [Command]
        public async void KickUser(SocketGuildUser user, [Remainder] string reason = null)
        {
            await user.KickAsync(reason);
            await ReplyAsync(_lang.GetMessage("Ban performed", user.Username));
        }
    }
}
