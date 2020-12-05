namespace Entity.RavenDB
{
    public class PermaChannel : Automation
    {
        public PermaChannel()
        {
            Enabled = true;
            Prefix = "👥";
            Name = "{0} channel";
        }

        public string Name { get; set; }
    }
}