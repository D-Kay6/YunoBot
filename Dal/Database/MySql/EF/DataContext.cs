using System.Threading.Tasks;
using Dal.Json;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace Dal.Database.MySql.EF
{
    public class DataContext : DbContext
    {
        private static string _connection;

        public DbSet<Server> Servers { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Ban> Bans { get; set; }

        public DbSet<LanguageSetting> LanguageSettings { get; set; }
        public DbSet<CommandSetting> CommandSettings { get; set; }
        public DbSet<CustomCommand> CustomCommands { get; set; }
        public DbSet<WelcomeMessage> WelcomeMessages { get; set; }

        public DbSet<AutoChannel> AutoChannels { get; set; }
        public DbSet<PermaChannel> PermaChannels { get; set; }
        public DbSet<GeneratedChannel> GeneratedChannels { get; set; }

        public DbSet<AutoRole> AutoRoles { get; set; }
        public DbSet<PermaRole> PermaRoles { get; set; }
        public DbSet<RoleIgnore> IgnoredUsers { get; set; }

        static DataContext()
        {
            var database = new Database<Connection>();
            var settings = database.Read();
            settings.Wait();
            _connection = settings.Result.CreateConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connection);
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();

            modelBuilder.Entity<CustomCommand>().HasKey(x => new { x.ServerId, x.Command });
            modelBuilder.Entity<Ban>().HasKey(x => new { x.UserId, x.ServerId });
            modelBuilder.Entity<GeneratedChannel>().HasKey(x => new { x.ServerId, x.ChannelId });
            modelBuilder.Entity<RoleIgnore>().HasKey(x => new { x.ServerId, x.UserId });
        }
    }
}