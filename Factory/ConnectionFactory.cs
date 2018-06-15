using ILogic.Interfaces;
using Logic;

namespace Factory
{
    public static class ConnectionFactory
    {
        public static IConnection GenerateConnection()
        {
            return new Connection();
        }
    }
}
