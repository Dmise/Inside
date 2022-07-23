using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


namespace Inside.Data
{
    public class InsideDbContext : DbContext
    {
        private readonly string _connectionString;
        public InsideDbContext() : base()
        {

        }
        public InsideDbContext(DbContextOptions<InsideDbContext> options) : base(options)
        {

        }
        public InsideDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var serverVesion = new MySqlServerVersion(new Version(5, 7));
        //    optionsBuilder.UseMySql(_connectionString, serverVesion);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Message>()
                .HasOne<User>(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.Username);
        }

        //entities
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
