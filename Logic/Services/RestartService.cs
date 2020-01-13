using System.Threading;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class RestartService
    {
        private readonly LogsService _logs;

        private CancellationTokenSource _restartToken;

        public bool KeepAlive { get; private set; }

        public RestartService(LogsService logs)
        {
            _logs = logs;
            KeepAlive = true;
        }

        public void Restart()
        {
            _restartToken.Cancel();
        }

        public void HardRestart()
        {
            //var path = ApplicationDeployment.CurrentDeployment.UpdatedApplicationFullName.Split('#')[0];
            //path = path.Remove(0, 8);
            //Console.WriteLine(path);
            //Process.Start(path);
            //this.Shutdown();
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
                await _logs.Write("Main", "Server is restarting.");
            }
        }
    }
}
