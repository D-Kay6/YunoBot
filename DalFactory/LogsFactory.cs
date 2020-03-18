namespace DalFactory
{
    using Dal.Text;
    using IDal;

    public static class LogsFactory
    {
        public static ILogs GenerateLogs()
        {
            return new Logs();
        }
    }
}