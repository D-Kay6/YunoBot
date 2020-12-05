namespace Entity.RavenDB
{
    public abstract class Automation
    {
        public bool Enabled { get; set; }
        public string Prefix { get; set; }
    }
}