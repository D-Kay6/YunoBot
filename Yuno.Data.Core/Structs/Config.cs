namespace Yuno.Data.Core.Structs
{
    public struct Config
    {
        public string Token;
        public string Prefix;

        public Config(string token = "", string prefix = "+")
        {
            this.Token = token;
            this.Prefix = prefix;
        }
    }
}
