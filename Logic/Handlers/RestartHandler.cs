using System.Threading;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class RestartHandler
    {
        private static RestartHandler _instance;

        public static RestartHandler Instance
        {
            get { return _instance ?? (_instance = new RestartHandler()); }
        }

        private CancellationTokenSource _restartToken;

        public RestartHandler()
        {
            KeepAlive = true;
        }

        public bool KeepAlive { get; private set; }

        public void Restart()
        {
            _restartToken.Cancel();
        }

        public void Shutdown()
        {
            KeepAlive = false;
            _restartToken.Cancel();
        }

        public async Task AwaitRestart()
        {
            _restartToken?.Dispose();
            _restartToken = new CancellationTokenSource();
            try
            {
                await Task.Delay(-1, _restartToken.Token);
            }
            catch (TaskCanceledException e)
            {
                LogsHandler.Instance.Log("Main", "Server is restarting.");
            }
        }
    }
}