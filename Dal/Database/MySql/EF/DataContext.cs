using Core.Entity;
using Dal.Json;
using Microsoft.EntityFrameworkCore;

namespace Dal.Database.MySql.EF
{
    public class DataContext : DbContext
    {
        private static readonly string _connection;

        static DataContext()
        {
            var database = new Database<Connection>();
            var settings = database.Read();
            settings.Wait();
            _connection = settings.Result.CreateConnectionString();
        }

        public DbSet<Server> Servers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Ban> Bans { get; set; }

        public DbSet<LanguageSetting> LanguageSettings { get; set; }
        public DbSet<CommandSetting> CommandSettings { get; set; }
        public DbSet<CustomCommand> CustomCommands { get; set; }
        public DbSet<WelcomeMessage> WelcomeMessages { get; set; }

        public DbSet<DynamicChannel> DynamicChannels { get; set; }
        public DbSet<GeneratedChannel> GeneratedChannels { get; set; }

        public DbSet<DynamicRole> DynamicRoles { get; set; }
        public DbSet<DynamicRoleData> DynamicRoleData { get; set; }
        public DbSet<DynamicRoleIgnore> IgnoredUsers { get; set; }

        public DbSet<ReactionRole> ReactionRoles { get; set; }
        public DbSet<ReactionRoleData> ReactionRoleData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connection, x => x.EnableRetryOnFailure());
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();

            modelBuilder.Entity<CustomCommand>().HasKey(x => new {x.ServerId, x.Command});
            modelBuilder.Entity<Ban>().HasKey(x => new {x.UserId, x.ServerId});
            modelBuilder.Entity<GeneratedChannel>().HasKey(x => new {x.ServerId, x.ChannelId});
            modelBuilder.Entity<DynamicRoleIgnore>().HasKey(x => new {x.ServerId, x.UserId});
            modelBuilder.Entity<DynamicRoleData>().HasKey(x => new {x.RoleId, x.DynamicRoleId});
            modelBuilder.Entity<ReactionRoleData>().HasKey(x => new {x.RoleId, x.ReactionRoleId});

            modelBuilder.Entity<DynamicChannel>().HasIndex(x => new {x.ServerId, x.Type}).IsUnique();
            modelBuilder.Entity<DynamicRole>().HasIndex(x => new {x.Type, x.Status}).IsUnique();
        }
    }
}