using Dal.Json.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logic.Configuration.Settings.Tests
{
    [TestClass]
    public class ConfigTests
    {
        [TestMethod]
        public void GetConfigTest()
        {
            var config = new Config(new ConfigDal());

            Assert.AreEqual(config.Token, "NDU1NzA4MjAwMjc3MTE0OTAx.Df_7Cw.Oqbtx-tsaedKwmlDVsvIvld4BLY", "The expected config token was not present.");
            Assert.AreEqual(config.Prefix, "$", "The expected config prefix was not present.");
        }
    }
}