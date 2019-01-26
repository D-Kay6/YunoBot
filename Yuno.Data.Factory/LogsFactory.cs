using Yuno.Data.Core.Interfaces;
using Yuno.Data.Text;

namespace Yuno.Data.Factory
{
    public static class LogsFactory
    {
        public static ILogs GenerateLogs()
        {
            return new Logs();
        }
    }
}
