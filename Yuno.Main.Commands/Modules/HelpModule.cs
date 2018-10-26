using System.Threading.Tasks;
using Discord.Commands;
using Yuno.Main.Extentions;

namespace Yuno.Main.Commands.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task Command([Remainder] string message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                await SendGeneralHelp();
                return;
            }

            switch (message)
            {
                case "praise":
                    var embed = EmbedExtention.CreateEmbed("Help praise", $@"Here is more information about 'praise'.

/praise (username) - Send a positive message to that user.
/praise someone - Send a positive message to a random person.
/praise everyone - Send a positive message to everyone.
/praise god - Send a positive message to god.
/praise satan - Send a positive message to satan.
/praise wizard - Send a positive message to the wizard.");
                    await ReplyAsync("", false, embed);
                    break;
                default:
                    await ReplyAsync("There is no advanced for that command (yet).");
                    break;
            }
        }

        private async Task SendGeneralHelp()
        {
            var ember = EmbedExtention.CreateEmbed("Help", $@"Here is a list of commands you can use.

/echo (message)- Repeat the message.
/pick (option1|option2|etc) - Select a random item from the options that are separated with |.
/birthday (username) - Sing a song for a happy fellow.
/praise (username) - Send a positive message to that user.
/kill (username) - Make me kill that user.
/help (command) - Show advanced help for a command.

I am constantly updated with new features so keep an eye on this page.");
            await ReplyAsync("", false, ember);
        }
    }
}
