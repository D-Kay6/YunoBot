namespace Dal.Database.RavenDB.Models
{
    public class PermaChannel : Automatization
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