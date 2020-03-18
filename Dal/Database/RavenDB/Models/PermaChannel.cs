namespace Dal.Database.RavenDB.Models
{
    public class PermaChannel : Automatization
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