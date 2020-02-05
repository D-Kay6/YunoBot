namespace Dal.Database.RavenDB.Models
{
    public class AutoRole : Automatization
    {
        public AutoRole()
        {
            this.Enabled = true;
            this.Prefix = "👾";
        }
    }
}