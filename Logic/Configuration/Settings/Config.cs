using IDal.Interfaces;
using ILogic.Interfaces;
using ILogic.Structs;

namespace Logic.Configuration.Settings
{
    public class Config : IConfig
    {
        private string _token;
        private string _prefix;
        
        public Config() { }

        public Config(IConfigDal dal)
        {
            var data = dal.Load();
            this._token = data.Token;
            this._prefix = data.Prefix;
        }

        public ConfigData GetConfig()
        {
            return new ConfigData(this._prefix, this._token);
        }
    }
}
