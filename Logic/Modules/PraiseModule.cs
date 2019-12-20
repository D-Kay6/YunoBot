using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;
using IDal.Interfaces.Database;
using Logic.Extensions;

namespace Logic.Modules
{
    [Group("praise")]
    public class PraiseModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public PraiseModule(IDbLanguage language)
        {
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

        // source: https://www.happier.com/blog/nice-things-to-say-100-compliments/
        private readonly List<string> _groupMessages = new List<string>
        {
            "{0}, you are awesome!",
            "{0}, you're all smart cookies.",
            "{0}, you all deserve a hug right now.",
            "{0}, you're more helpful than you all realize.",
            "{0}, you all light up the room.",
            "I'm inspired by you all, {0}.",
            "Everything would be better if more people were like you all, {0}!",
            "You're all making a difference, {0}.",
            "You all should be proud of yourself, {0}."

            /*"{0}, you’re off to great places, today is your day. Your mountain is waiting, so get on your way.",
            "{0}, you’re braver than you believe, and stronger than you seem, and smarter than you think.",
            "If the world had more people like you {0}, it would be a better place. You do make a difference.",
            "you're a gift to those around you, {0}.",
            "{0}, you have impeccable manners.",
            "{0}, you are the most perfect you there is.",
            "Your perspective is refreshing, {0}.",
            "{0}, your kindness is a balm to all who encounter it.",
            "{0}, you're all that and a super-size bag of chips.",
            "On a scale from 1 to 10, you're an 11, {0}.",
            "{0}, you're even more beautiful on the inside than you are on the outside.",
            "{0}, you have the courage of your convictions.",
            "{0}, you're like a ray of sunshine on a really dreary day.",
            "{0}, your ability to recall random factoids at just the right time is impressive.",
            "You're a great listener, {0}.",
            "How is it that you always look great, even in sweatpants, {0}?",
            "{0}, I bet you sweat glitter.",
            "You were cool way before hipsters were cool, {0}.",
            "That color is perfect on you, {0}.",
            "Hanging out with you is always a blast, {0}."*/
        };

        private readonly List<string> _soloMessages = new List<string>
        {
            "Good job, {0}. You deserve a :cookie:.",
            "{0}, you’re off to great places, today is your day. Your mountain is waiting, so get on your way.",
            "{0}, you’re braver than you believe, and stronger than you seem, and smarter than you think.",
            "If the world had more people like you {0}, it would be a better place. You do make a difference.",

            //"{0}, you're an awesome friend.",
            "you're a gift to those around you, {0}.",
            "{0}, you're a smart cookie.",
            "{0}, you are awesome!",
            "{0}, you have impeccable manners.",
            //"I like your style, {0}.",
            //"{0}, you have the best laugh.",
            //"{0}, I appreciate you.",
            "{0}, you are the most perfect you there is.",
            //"{0}, you are enough.",
            //"{0}, you're strong.",
            "Your perspective is refreshing, {0}.",
            "{0}, you light up the room.",
            "{0}, you deserve a hug right now.",
            "You should be proud of yourself, {0}.",
            "{0}, you're more helpful than you realize.",
            //"{0}, you have a great sense of humor.",
            //"{0}, you've got an awesome sense of humor!",
            //"{0}, you are really courageous.",
            "{0}, your kindness is a balm to all who encounter it.",
            "{0}, you're all that and a super-size bag of chips.",
            "On a scale from 1 to 10, you're an 11, {0}.",
            //"{0}, you are strong.",
            "{0}, you're even more beautiful on the inside than you are on the outside.",
            "{0}, you have the courage of your convictions.",
            "I'm inspired by you, {0}.",
            "{0}, you're like a ray of sunshine on a really dreary day.",
            "You are making a difference, {0}.",
            //"{0}, thank you for being there for me.",
            //"{0}, you bring out the best in other people.",

            "{0}, your ability to recall random factoids at just the right time is impressive.",
            "You're a great listener, {0}.",
            "How is it that you always look great, even in sweatpants, {0}?",
            "Everything would be better if more people were like you, {0}!",
            "{0}, I bet you sweat glitter.",
            "You were cool way before hipsters were cool, {0}.",
            "That color is perfect on you, {0}.",
            "Hanging out with you is always a blast, {0}."
            //"You always know -- and say -- exactly what I need to hear when I need to hear it.",
            //"You help me feel more joy in life.",
            //"You may dance like no one's watching, but everyone's watching because you're an amazing dancer!",
            //"Being around you makes everything better!",
            //"When you say, \"I meant to do that,\" I totally believe you.",
            //"When you're not afraid to be yourself is when you're most incredible.",
            //"Colors seem brighter when you're around.",
            //"You're more fun than a ball pit filled with candy. (And seriously, what could be more fun than that?)",
            //"That thing you don't like about yourself is what makes you so interesting.",
            //"You're wonderful.",
            //"You have cute elbows. For reals!",
            //"Jokes are funnier when you tell them.",
            //"You're better than a triple-scoop ice cream cone. With sprinkles.",
            //"When I'm down you always say something encouraging to help me feel better.",
            //"You are really kind to people around you.",
            //"You're one of a kind!",
            //"You help me be the best version of myself.",
            //"If you were a box of crayons, you'd be the giant name-brand one with the built-in sharpener.",
            //"You should be thanked more often. So thank you!!",
            //"Our community is better because you're in it.",
            //"Someone is getting through something hard right now because you've got their back.",
            //"You have the best ideas.",
            //"You always find something special in the most ordinary things.",
            //"Everyone gets knocked down sometimes, but you always get back up and keep going.",
            //"You're a candle in the darkness.",
            //"You're a great example to others.",
            //"Being around you is like being on a happy little vacation.",
            //"You always know just what to say.",
            //"You're always learning new things and trying to better yourself, which is awesome.",
            //"If someone based an Internet meme on you, it would have impeccable grammar.",
            //"You could survive a Zombie apocalypse.",
            //"You're more fun than bubble wrap.",
            //"When you make a mistake, you try to fix it.",
            //"Who raised you? They deserve a medal for a job well done.",
            //"You're great at figuring stuff out.",
            //"Your voice is magnificent.",
            //"The people you love are lucky to have you in their lives.",
            //"You're like a breath of fresh air.",
            //"You make my insides jump around in the best way.",
            //"You're so thoughtful.",
            //"Your creative potential seems limitless.",
            //"Your name suits you to a T.",
            //"Your quirks are so you -- and I love that.",
            //"When you say you will do something, I trust you.",
            //"Somehow you make time stop and fly at the same time.",
            //"When you make up your mind about something, nothing stands in your way.",
            //"You seem to really know who you are.",
            //"Any team would be lucky to have you on it.",
            //"In high school I bet you were voted \"most likely to keep being awesome.\"",
            //"I bet you do the crossword puzzle in ink.",
            //"Babies and small animals probably love you.",
            //"If you were a scented candle they'd call it Perfectly Imperfect (and it would smell like summer).",
            //"There's ordinary, and then there's you.",
            //"You're someone's reason to smile.",
            //"You're even better than a unicorn, because you're real.",
            //"How do you keep being so funny and making everyone laugh?",
            //"You have a good head on your shoulders.",
            //"Has anyone ever told you that you have great posture?",
            //"The way you treasure your loved ones is incredible.",
            //"You're really something special.",
            //"Thank you for being you."
        };

        [Priority(-1)]
        [Command]
        public async Task DefaultPraise([Remainder] string name)
        {
            await Context.Channel.SendMessageAsync(_lang.GetMessage("Invalid user", name));
        }

        [Command]
        public async Task DefaultPraise(SocketGuildUser user)
        {
            await PraiseUser(user);
        }

        [Command("role")]
        private async Task PraiseRole([Remainder] string name)
        {
            await Context.Channel.SendMessageAsync(_lang.GetMessage("Invalid role", name));
        }

        [Command("role")]
        private async Task PraiseRole(SocketRole role)
        {
            await PraiseGroup(role);
        }

        private async Task PraiseUser(SocketGuildUser user)
        {
            if (user == null) return;
            switch (user.Id)
            {
                case 255453041531158538:
                    await PraiseDKay();
                    return;
                case 235587420777611265:
                    await PraiseDemanicus();
                    return;
                case 283233504156975105:
                    await PraiseJim();
                    return;
                default:
                    await ReplyAsync(string.Format(_lang.GetRandomUserPraise(), user.Mention));
                    return;
            }
        }

        private async Task PraiseGroup(SocketRole role)
        {
            await ReplyAsync(string.Format(_lang.GetRandomGroupPraise(), role.Mention));
        }

        [Command("everyone")]
        public async Task PraiseEveryone()
        {
            await PraiseGroup(Context.Guild.EveryoneRole);
        }

        [Command("someone")]
        public async Task PraiseSomeone()
        {
            var user = Context.Channel.GetRandomUser();
            await PraiseUser(user);
        }

        [Command("creator")]
        [Alias("D-Kay")]
        public async Task PraiseDKay()
        {
            await ReplyAsync(_lang.GetMessage("Praise creator"));
        }

        [Command("god")]
        public async Task PraiseDemanicus()
        {
            await ReplyAsync("Wait, are you sure you mean him? Ok then. \nAll hail our lord and savior, Demanicus.");
        }

        [Command("wizard")]
        public async Task PraiseJim()
        {
            await ReplyAsync("I cannot imagine he deserves it but, \nAll hail our powerful and destructive wizard, jim.");
        }
    }
}