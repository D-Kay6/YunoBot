using System.Threading;
using System.Threading.Tasks;
using Logic.Handlers;

namespace Logic.Services
{
    public class RestartService
    {
        private CancellationTokenSource _restartToken;

        public bool KeepAlive { get; private set; }

        public RestartService()
        {
            KeepAlive = true;
        }

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
            catch (TaskCanceledException)
            {
                LogService.Instance.Log("Main", "Server is restarting.");
            }
        }
    }
}
