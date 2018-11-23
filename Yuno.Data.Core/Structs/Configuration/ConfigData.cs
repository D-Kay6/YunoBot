namespace Yuno.Data.Core.Structs
{
    public struct ConfigData
    {
        public string Token;
        public string Prefix;

        public ConfigData(string token = "", string prefix = "+")
        {
            this.Token = token;
            this.Prefix = prefix;
        }
    }
}
