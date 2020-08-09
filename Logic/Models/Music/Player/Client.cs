using Logic.Exceptions;
using Logic.Models.Music.Event;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Logic.Models.Music.Player
{
    public class Client
    {
        private const string _javaHome = "JAVA_HOME";
        private const string _parameters = "-jar \"{0}\"";
        private string JavaExecutable => Path.Combine("bin", "java.exe");
        private string LavalinkExecutable => Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents",
            "Yuno Bot", "Lavalink", "Lavalink.jar");

        public event Func<LavalinkEventArgs, Task> ClientException;

        public bool IsActive { get; private set; }

        private int _lastProcessId;
        private Process _process;

        /// <summary>
        /// Start the client process.
        /// </summary>
        /// <exception cref="InvalidStateException">Thrown if the client is already active.</exception>
        /// <exception cref="ArgumentException">Thrown if JAVA_HOME is not setup properly.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the client could not be started.</exception>
        public Task Start()
        {
            if (IsActive)
                throw new InvalidStateException("The process is already running.");

            var javaHome = Environment.GetEnvironmentVariable(_javaHome);
            if (string.IsNullOrEmpty(javaHome))
                throw new ArgumentException("Java home directory could not be found.");

            var processStartInfo = new ProcessStartInfo(Path.Combine(javaHome,JavaExecutable), string.Format(_parameters, LavalinkExecutable))
            {
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                RedirectStandardInput = false,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _process = Process.Start(processStartInfo);
            if (_process == null)
                throw new InvalidOperationException("The process could not be started.");

            _lastProcessId = _process.Id;
            //_process.BeginErrorReadLine();
            //_process.BeginOutputReadLine();
            //_process.OutputDataReceived += OnOutputReceived;
            //_process.ErrorDataReceived += OnErrorReceived;

            _lastProcessId = _process.Id;
            IsActive = true;

            return Task.CompletedTask;
        }

        ///// <summary>
        ///// Start the client process.
        ///// </summary>
        ///// <exception cref="InvalidStateException">Thrown if the client is already active.</exception>
        ///// <exception cref="ArgumentException">Thrown if JAVA_HOME is not setup properly.</exception>
        ///// <exception cref="InvalidOperationException">Thrown if the client could not be started.</exception>
        //public async Task Start()
        //{
        //    if (IsActive)
        //        throw new InvalidStateException("The process is already running.");

        //    //var javaHome = Environment.GetEnvironmentVariable(_javaHome);
        //    //if (string.IsNullOrEmpty(javaHome))
        //    //    throw new ArgumentException("Java home directory could not be found.");

        //    var processStartInfo = new ProcessStartInfo("cmd.exe")
        //    {
        //        RedirectStandardError = false,
        //        RedirectStandardOutput = false,
        //        RedirectStandardInput = true,
        //        UseShellExecute = false,
        //        CreateNoWindow = false
        //    };

        //    _process = Process.Start(processStartInfo);
        //    if (_process == null)
        //        throw new InvalidOperationException("The process could not be started.");

        //    _lastProcessId = _process.Id;
        //    //_process.BeginErrorReadLine();
        //    //_process.BeginOutputReadLine();
        //    //_process.OutputDataReceived += OnOutputReceived;
        //    //_process.ErrorDataReceived += OnErrorReceived;

        //    await _process.StandardInput.WriteLineAsync($"java {string.Format(_parameters, LavalinkExecutable)}");

        //    _lastProcessId = _process.Id;
        //    IsActive = true;

        //    //return Task.CompletedTask;
        //}

        /// <summary>
        /// Stops the client from operating.
        /// </summary>
        /// <exception cref="InvalidStateException">Thrown if the client is not active.</exception>
        public async Task Stop()
        {
            if (!IsActive)
                throw new InvalidStateException("The process is not running.");

            _process.Close();
            _process.Dispose();

            _process = Process.GetProcessById(_lastProcessId);
            _process?.Kill();

            _process = null;
            IsActive = false;

            //return Task.CompletedTask;
        }

        private async void OnOutputReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;
            Console.WriteLine(e.Data);
            if (!e.Data.Contains("java.io.IOException")) return;

            if (ClientException == null) return;
            await ClientException.Invoke(new LavalinkEventArgs(e.Data));
        }

        private async void OnErrorReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;
            Console.WriteLine(e.Data);
            if (!e.Data.Contains("java.io.IOException")) return;

            if (ClientException == null) return;
            await ClientException.Invoke(new LavalinkEventArgs(e.Data));
        }
    }
}