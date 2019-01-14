using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
