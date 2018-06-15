using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.Json;
using IDal.Interfaces;
using IDal.Structs;

namespace DalFactory
{
    public static class ConfigFactory
    {
        public static IConfigDal GenerateConfig()
        {
            return new ConfigDal();
        }
    }
}
