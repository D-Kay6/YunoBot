namespace Entity.RavenDB
{
    public abstract class Automatization
    {
        public bool Enabled { get; set; }
        public string Prefix { get; set; }
    }
}