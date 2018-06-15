using IDal.Interfaces;

namespace Logic.Configuration.Settings
{
    public class Config
    {
        public string Token { get; private set; }
        public string Prefix { get; private set; }

        public Config() { }

        public Config(IConfigDal dal)
        {
            var data = dal.Load();
            this.Token = data.Token;
            this.Prefix = data.Prefix;
        }
    }
}
