using Yuno.Main.Core;

namespace Yuno.Main.Factory
{
    public class BotFactory
    {
        public static IBot GenerateBot()
        {
            return new Bot();
        }
    }
}
