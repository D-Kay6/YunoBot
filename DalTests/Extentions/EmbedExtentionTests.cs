using Discord;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logic.Extentions.Tests
{
    [TestClass]
    public class EmbedExtentionTests
    {
        [TestMethod]
        public void CreateEmbedTest()
        {
            const string title = "Test";
            const string msg = "This is a test.";
            var color = Color.Blue;

            var builder = new EmbedBuilder();
            builder.WithTitle(title);
            builder.WithColor(color);
            builder.WithDescription(msg);
            var build = builder.Build();

            var embed = EmbedExtention.CreateEmbed(title, msg, color);
            Assert.AreEqual(embed.Title, build.Title, "The embed extention did not create the proper embed.");
        }
    }
}