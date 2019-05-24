using System;

namespace IDal.Structs.Database
{
    public class RoleData
    {
        public string AutoPrefix { get; private set; }
        public string PermaPrefix { get; private set; }

        public RoleData(string autoPrefix, string permaPrefix)
        {
            this.AutoPrefix = autoPrefix;
            this.PermaPrefix = permaPrefix;
        }
    }
}
