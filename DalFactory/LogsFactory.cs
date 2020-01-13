using Dal.Text;
using IDal;

namespace DalFactory
{
    public static class LogsFactory
    {
        public static ILogs GenerateLogs()
        {
            return new Logs();
        }
    }
}
