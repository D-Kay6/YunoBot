using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Logic.Extentions;

namespace Logic.Commands.Modules
{
    public class AlertCommand : ModuleBase<SocketCommandContext>
    {
        private bool _isGlobal = true;
        private bool _isOwner = false;
        private HashSet<ulong> _usersDone;
        
        [Command("alert")]
        public async Task CommandAlert([Remainder] string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            message = HandleArgs(message.Split(' '));
            await SendAlert(message);
        }

        public string HandleArgs(string[] args)
        {
            for (var i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "global":
                        _isGlobal = true;
                        break;
                    case "here":
                        _isGlobal = false;
                        break;
                    case "all":
                        _isOwner = false;
                        break;
                    case "owner":
                        _isOwner = true;
                        break;
                    default:
                        return string.Join(" ", args.Skip(i));
                }
            }
            return null;
        }

        private async Task SendAlert(string message)
        {
            _usersDone = new HashSet<ulong>();
            if (_isGlobal)
            {
                Context.Client.Guilds.ForEach(async g => await SendAlert(g, message));
            }
            else
            {
                await SendAlert(Context.Guild, message);
            }
        }

        private async Task SendAlert(SocketGuild guild, string message)
        {
            if (_isOwner)
            {
                var user = guild.Owner;
                await SendDM(user, message);
                return;
            }
            guild.Users.ForEach(async u => await SendDM(u, message));
        }

        private async Task SendDM(SocketUser user, string message)
        {
            if (_usersDone.Contains(user.Id)) return;
            var channel = await user.GetOrCreateDMChannelAsync();
            await channel.SendMessageAsync(message);
            _usersDone.Add(user.Id);
        }
    }
}
