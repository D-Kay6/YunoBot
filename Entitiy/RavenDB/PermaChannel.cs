namespace Entity.RavenDB
{
    public class PermaChannel : Automation
    {
        public string Name { get; set; }

        public PermaChannel()
        {
            this.Enabled = true;
            this.Prefix = "👥";
            this.Name = "{0} channel";
        }
    }
}