using ILogic.Interfaces;
using Logic.Channels;
using Logic.Commands;

namespace Factory
{
    public static class HandlerFactory
    {
        public static IHandler GenerateCommandHandler()
        {
            return new CommandHandler();
        }

        public static IHandler GenerateChannelHandler()
        {
            return new ChannelHandler();
        }
    }
}
