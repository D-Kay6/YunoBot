using ILogic;
using Logic;

namespace LogicFactory
{
    public class BotFactory
    {
        public static IBot GenerateBot()
        {
            return new Bot();
        }
    }
}
