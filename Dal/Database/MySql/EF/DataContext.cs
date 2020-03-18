namespace Dal.Database.MySql.EF
{
    using Core.Entity;
    using Json;
    using Microsoft.EntityFrameworkCore;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connection, x => x.EnableRetryOnFailure());
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();

            modelBuilder.Entity<CustomCommand>().HasKey(x => new {x.ServerId, x.Command});
            modelBuilder.Entity<Ban>().HasKey(x => new {x.UserId, x.ServerId});
            modelBuilder.Entity<GeneratedChannel>().HasKey(x => new {x.ServerId, x.ChannelId});
            modelBuilder.Entity<RoleIgnore>().HasKey(x => new {x.ServerId, x.UserId});
        }
    }
}