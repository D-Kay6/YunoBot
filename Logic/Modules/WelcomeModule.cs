using System.Linq;
using System.Threading.Tasks;
using DalFactory;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Logic.Extentions;

namespace Logic.Modules
{
    [Group("welcome")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class WelcomeModule : ModuleBase<SocketCommandContext>
    {
        [Priority(-1)]
        [Command]
        public async Task DefaultWelcome([Remainder] string name)
        {
            await Context.Channel.SendMessageAsync($"Wait, who do you mean? I cannot find `{name}`.");
        }
        
        [Command]
        public async Task DefaultWelcome(params SocketGuildUser[] users)
        {
            var welcome = DatabaseFactory.GenerateWelcomeMessage();
            var settings = welcome.GetWelcomeMessage(Context.Guild.Id);
            if (settings == null)
            {
                await ReplyAsync("No welcome settings could be found. Please go to my support discord to have this fixed.");
                return;
            }
            var names = string.Join(", ", users.Select(u => u.Mention)).ReplaceLast(", ", " and ");
            var msg = string.Format(settings.Message, names);

            if (!settings.UseImage) await ReplyAsync(msg);
            else await Context.Channel.SendFileAsync(ImageExtentions.GetImagePath("GasaiYunoWelcome.jpg"), msg);
        }
        
        [Command("enable")]
        [Alias("on")]
        public async Task WelcomeEnable(SocketTextChannel channel)
        {
            var welcome = DatabaseFactory.GenerateWelcomeMessage();
            welcome.Enable(Context.Guild.Id, channel.Id);
            await ReplyAsync($"I will now welcome any new member in `{channel.Name}`.");
        }
        
        [Command("disable")]
        [Alias("off")]
        public async Task WelcomeDisable()
        {
            var welcome = DatabaseFactory.GenerateWelcomeMessage();
            welcome.Disable(Context.Guild.Id);
            await ReplyAsync("The welcome message has now been disabled. You can still send a welcome message with my normal command.");
        }

        [Group("message")]
        public class WelcomeMessageModule : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task DefaultWelcomeMessage()
            {
                var welcome = DatabaseFactory.GenerateWelcomeMessage();
                await ReplyAsync($@"The welcome message for {Context.Guild.Name} is `{welcome.GetWelcomeMessage(Context.Guild.Id)}`.
When setting your custom welcome image, you can use `" + "{0}" + "` as a variable where the mention of the user is placed.");
            }

            [Command("set")]
            public async Task WelcomeMessageSet([Remainder] string message)
            {
                var welcome = DatabaseFactory.GenerateWelcomeMessage();
                welcome.SetWelcomeMessage(Context.Guild.Id, message);
                await ReplyAsync($@"The welcome for {Context.Guild.Name} was changed to:
```
{message}
```");
            }
        }

        [Group("image")]
        public class WelcomeImageModule : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task DefaultWelcomeImage()
            {
                var welcome = DatabaseFactory.GenerateWelcomeMessage();
                var settings = welcome.GetWelcomeMessage(Context.Guild.Id);
                if (settings == null)
                {
                    await ReplyAsync("No welcome settings could be found. Please go to my support discord to have this fixed.");
                    return;
                }

                var useImage = settings.UseImage ? "use" : "not use";
                await ReplyAsync($"I am currently set to { useImage} the standard welcome image.");
            }

            [Command("enable")]
            [Alias("on")]
            public async Task WelcomeImageEnable()
            {
                var welcome = DatabaseFactory.GenerateWelcomeMessage();
                welcome.UseImage(Context.Guild.Id, true);
                await ReplyAsync($"The welcome image for {Context.Guild.Name} will be shown in welcome messages.");
            }

            [Command("disable")]
            [Alias("off")]
            public async Task WelcomeImageDisable()
            {
                var welcome = DatabaseFactory.GenerateWelcomeMessage();
                welcome.UseImage(Context.Guild.Id, false);
                await ReplyAsync($"The welcome image for {Context.Guild.Name} will not be shown in welcome messages.");
            }
        }
    }
}
