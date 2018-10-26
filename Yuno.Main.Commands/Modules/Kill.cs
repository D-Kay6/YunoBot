using System.Threading.Tasks;
using Discord.Commands;

namespace Yuno.Main.Commands.Modules
{
    public class Kill : ModuleBase<SocketCommandContext>
    {
        [Command("kill")]
        public async Task Command([Remainder] string message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                SendHelpText();
                return;
            }
        }

        private void SendHelpText()
        {
            var channel = Context.Channel;
            channel.SendMessageAsync($@"/kill (username) - Order me to kill someone.");
        }
    }
}
