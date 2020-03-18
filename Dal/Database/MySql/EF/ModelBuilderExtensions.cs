namespace Dal.Database.MySql.EF
{
    using Microsoft.EntityFrameworkCore;

    public static class ModelBuilderExtensions
    {
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes()) entity.SetTableName(entity.DisplayName());
        }
    }
}