using System.Threading.Tasks;

namespace Logic.Handlers
{
    public static class RestartHandler
    {
        private static bool _restart;

        static RestartHandler()
        {
            _restart = false;
            KeepAlive = true;
        }

        public static bool KeepAlive { get; private set; }

        public static void Restart()
        {
            _restart = true;
        }

        public static void Shutdown()
        {
            KeepAlive = false;
            _restart = true;
        }

        public static async Task AwaitRestart()
        {
            _restart = false;
            while (!_restart) await Task.Delay(1);
        }
    }
}