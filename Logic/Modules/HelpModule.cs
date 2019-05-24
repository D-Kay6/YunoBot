using Discord.Commands;
using Logic.Data;
using Logic.Extentions;
using System.Threading.Tasks;
using DalFactory;
using Discord;

namespace Logic.Modules
{
    [Group("help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Priority(-1)]
        [Command]
        public async Task DefaultHelp([Remainder] string message = null)
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            if (!string.IsNullOrEmpty(message))
            {
                await ReplyAsync("There is no advanced information for that command (yet).");
                return;
            }

            var embed = new EmbedBuilder();
            embed.AddField("Help", @"Here is a list of commands you can use.
I am constantly updated with new features so keep an eye on this page.");

            embed.AddField("__General Commands__", $@"{prefix}help <command> - Show advanced help for a command.
{prefix}invite - Ask me for my invite link.
{prefix}support - Join this discord if you experience problems with me.
{prefix}pick <option1|option2|etc> - Select a random item from the options that are separated with |.
{prefix}birthday <username> - Sing a song for a happy fellow.
{prefix}kill <username> - Make me kill that user.");

            embed.AddField("Praise", $@"{prefix}praise <username> - Send a positive message to that user.
{prefix}praise someone - Send a positive message to a random person.
{prefix}praise everyone - Send a positive message to everyone.
{prefix}praise role <rolename> - Send a positive message to a group.");

            embed.AddField("music", $@"{prefix}music play <name> - Search for a song by it's name and add it to the queue.
{prefix}music play <video url> - Add a song to the queue.
{prefix}music play <playlist url> - Add an entire playlist to the queue.
{prefix}music pause - Pause the currently playing song.
{prefix}music unpause - Un-pause the currently playing song.
{prefix}music playing - Show what song is currently playing.
{prefix}music queue - List all the songs in the queue.
{prefix}music volume <value> - Change my music volume. (admin only)
{prefix}music shuffle - Shuffle all the songs in the queue.
{prefix}music skip - Skip the song currently playing.
{prefix}music clear - Remove all songs from the queue.
{prefix}music stop - Stop playing music and remove all songs from the queue.");

            embed.AddField("request", $@"{prefix}request feature <message> - Send a message to my developer with the request for a new feature.
{prefix}request change <message> - Send a message to my developer with the request for a change of an already existing feature.");

            embed.AddField("__Admin Commands__", $@"{prefix}announce <message> - Announce a message to all members of your server.");

            embed.AddField("prefix", $@"{prefix}prefix - Show the command prefix for this server.
{prefix}prefix set <new prefix> - Change the command prefix for this server.");

            embed.AddField("welcome", $@"{prefix}welcome <username, username, etc> - Send a welcome message to one or multiple users.
{prefix}welcome enable/on <textchannel> - Turn automated welcome messages on. Messages will be send in the provided text channel.
{prefix}welcome disable/off - Turn the automated welcome messages off.
{prefix}welcome message - Display the current welcome message.
{prefix}welcome message set <message> - Set a custom welcome message.
{prefix}welcome image - Display if the default image is being shown in welcome messages.
{prefix}welcome image enable/on - Set the default image to be shown in welcome messages.
{prefix}welcome image disable/off - Set the default image to not be shown in welcome messages.");

            embed.AddField("autochannel (ac)", $@"{prefix}autochannel - Show information about auto channels.
{prefix}autochannel prefix - Show the current prefix for auto channels.
{prefix}autochannel prefix set <new prefix> - Change the prefix for auto channels.
{prefix}autochannel name - Show the current name for auto generated channels.
{prefix}autochannel name set <new name> - Change the name for auto  generated channels.
{prefix}autochannel delete - Deletes all voice channels that still have the name for auto generated channels.");

            embed.AddField("permachannel (pc)", $@"{prefix}permachannel - Show information about perma channels.
{prefix}permachannel prefix - Show the current prefix for perma channels.
{prefix}permachannel prefix set <new prefix> - Change the prefix for perma channels.");

            embed.AddField("autorole (ar)", $@"{prefix}autorole - Show information about auto roles.
{prefix}autorole prefix - Show the current prefix for auto roles.
{prefix}autorole prefix set <new prefix> - Change the prefix for auto roles.");

            embed.AddField("permarole (pr)", $@"{prefix}permarole - Show information about perma roles.
{prefix}permarole prefix - Show the current prefix for perma roles.
{prefix}permarole prefix set <new prefix> - Change the prefix for perma roles.");

            await ReplyAsync("", false, embed.Build());
        }

        [Command("praise")]
        public async Task HelpPraise()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help praise", $@"Here is more information about 'praise'.

{prefix}praise <username> - Send a positive message to that user.
{prefix}praise someone - Send a positive message to a random person.
{prefix}praise everyone - Send a positive message to everyone.");
            await ReplyAsync("", false, embed);
        }

        [Command("music")]
        public async Task HelpMusic()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help music", $@"Here is more information about 'music'.

{prefix}music play <name> - Search for a song by it's name and add it to the queue.
{prefix}music play <video url> - Add a song to the queue.
{prefix}music play <playlist url> - Add an entire playlist to the queue.
{prefix}music pause - Pause the currently playing song.
{prefix}music unpause - Un-pause the currently playing song.
{prefix}music playing - Show what song is currently playing.
{prefix}music queue - List all the songs in the queue.
{prefix}music volume <value> - Change my music volume. (admin only)
{prefix}music shuffle - Shuffle all the songs in the queue.
{prefix}music skip - Skip the song currently playing.
{prefix}music clear - Remove all songs from the queue.
{prefix}music stop - Stop playing music and remove all songs from the queue.");
            await ReplyAsync("", false, embed);
        }

        [Command("request")]
        public async Task HelpRequest()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help request", $@"Here is more information about 'request'.

{prefix}request feature <message> - Send a message to my developer with the request for a new feature.
{prefix}request change <message> - Send a message to my developer with the request for a change of an already existing feature.");
            await ReplyAsync("", false, embed);
        }

        [Command("prefix")]
        public async Task HelpPrefix()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help prefix", $@"Here is more information about 'prefix'.

{prefix}prefix - Show the command prefix for this server.
{prefix}prefix set <new prefix> - Change the command prefix for this server.");
            await ReplyAsync("", false, embed);
        }

        [Command("welcome")]
        public async Task HelpWelcome()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help welcome", $@"Here is more information about 'welcome'.

{prefix}welcome <username, username, etc> - Send a welcome message to one or multiple users.
{prefix}welcome enable/on <textchannel> - Turn automated welcome messages on. Messages will be send in the provided text channel.
{prefix}welcome disable/off - Turn the automated welcome messages off.
{prefix}welcome message - Display the current welcome message.
{prefix}welcome message set <message> - Set a custom welcome message.
{prefix}welcome image - Display if the default image is being shown in welcome messages.
{prefix}welcome image enable/on - Set the default image to be shown in welcome messages.
{prefix}welcome image disable/off - Set the default image to not be shown in welcome messages.");
            await ReplyAsync("", false, embed);
        }

        [Alias("ac")]
        [Command("autochannel")]
        public async Task HelpAutoChannel()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help autochannel", $@"Here is more information about 'autochannel'.

{prefix}autochannel - Show information about auto channels.
{prefix}autochannel prefix - Show the current prefix for auto channels.
{prefix}autochannel prefix set <new prefix> - Change the prefix for auto channels.
{prefix}autochannel name - Show the current name for auto generated channels.
{prefix}autochannel name set <new name> - Change the name for auto  generated channels.
{prefix}autochannel delete - Deletes all voice channels that still have the name for auto generated channels.");
            await ReplyAsync("", false, embed);
        }

        [Alias("pc")]
        [Command("permachannel")]
        public async Task HelpPermaChannel()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help permachannel",
                $@"Here is more information about 'permachannel'.

{prefix}permachannel - Show information about perma channels.
{prefix}permachannel prefix - Show the current prefix for perma channels.
{prefix}permachannel prefix set <new prefix> - Change the prefix for perma channels.");
            await ReplyAsync("", false, embed);
        }

        [Alias("ar")]
        [Command("autorole")]
        public async Task HelpAutoRole()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help autorole", $@"Here is more information about 'autorole'.

{prefix}autorole - Show information about auto roles.
{prefix}autorole prefix - Show the current prefix for auto roles.
{prefix}autorole prefix set <new prefix> - Change the prefix for auto roles.");
            await ReplyAsync("", false, embed);
        }

        [Alias("pr")]
        [Command("permarole")]
        public async Task HelpPermaRole()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help permarole", $@"Here is more information about 'permarole'.

{prefix}permarole - Show information about perma roles.
{prefix}permarole prefix - Show the current prefix for perma roles.
{prefix}permarole prefix set <new prefix> - Change the prefix for perma roles.");
            await ReplyAsync("", false, embed);
        }
    }
}