using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logic.Commands.Modules
{
    [TestClass]
    public class AlertCommandTests
    {
        [TestMethod]
        public void HandleAllArgsTest()
        {
            const string command = "here owner Testing the behaviour of argument splitting.";
            var args = command.Split(' ');

            var alertCommand = new AlertCommand();
            Assert.AreEqual(alertCommand.HandleArgs(args), "Testing the behaviour of argument splitting.", "The command argument handler did not remove all arguments.");
        }

        [TestMethod]
        public void HandleOneArgsTest()
        {
            const string command = "owner Testing the behaviour of argument splitting.";
            var args = command.Split(' ');

            var alertCommand = new AlertCommand();
            Assert.AreEqual(alertCommand.HandleArgs(args), "Testing the behaviour of argument splitting.", "The command argument handler did not remove the argument.");
        }

        [TestMethod]
        public void HandleNoArgsTest()
        {
            const string command = "Testing the behaviour of argument splitting.";
            var args = command.Split(' ');

            var alertCommand = new AlertCommand();
            Assert.AreEqual(alertCommand.HandleArgs(args), "Testing the behaviour of argument splitting.", "The command argument handler did not return the complete message.");
        }
    }
}