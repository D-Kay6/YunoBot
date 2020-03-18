namespace LogicFactory
{
    using ILogic;
    using Logic;

    public class BotFactory
    {
        public static IBot GenerateBot()
        {
            return new Bot();
        }
    }
}