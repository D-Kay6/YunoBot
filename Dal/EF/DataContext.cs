using Dal.Json;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF
{
    public class DataContext : DbContext
    {
        public DbSet<Server> Servers { get; set; }

        public DbSet<LanguageSetting> LanguageSettings { get; set; }
        public DbSet<CommandSetting> CommandSettings { get; set; }
        public DbSet<WelcomeMessage> WelcomeMessages { get; set; }

        public DbSet<AutoChannel> AutoChannels { get; set; }
        public DbSet<PermaChannel> PermaChannels { get; set; }
        public DbSet<GeneratedChannel> GeneratedChannels { get; set; }

        public DbSet<AutoRole> AutoRoles { get; set; }
        public DbSet<PermaRole> PermaRoles { get; set; }
        public DbSet<RoleIgnore> IgnoredUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionstring = new Database().Read().CreateConnectionString();
            optionsBuilder.UseMySql(connectionstring);
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();

            modelBuilder.Entity<GeneratedChannel>().HasKey(x => new { x.ServerId, x.ChannelId });
            modelBuilder.Entity<RoleIgnore>().HasKey(x => new { x.ServerId, x.UserId });
        }
    }
}
