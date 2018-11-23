using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yuno.Main.Restart
{
    public static class RestartHandler
    {
        private static bool _restart;
        public static bool KeepAlive { get; private set; }

        static RestartHandler()
        {
            _restart = false;
            KeepAlive = true;
        }

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
            while (!_restart)
            {
                await Task.Delay(1);
            }
        }
    }
}
