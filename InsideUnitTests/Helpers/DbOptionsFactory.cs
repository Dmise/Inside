using Inside.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsideUnitTests.Helpers
{
    internal static class DbOptionsFactory
    {
        static DbOptionsFactory()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = config["ConnectionStrings:dmise.dev"];
            ConnectionString = connectionString;
            var serverVesion = new MySqlServerVersion(new Version(5, 7));
            DbContextOptions = new DbContextOptionsBuilder<InsideDbContext>()
                .UseMySql(connectionString, serverVesion)
                .Options;
            
        }

        public static DbContextOptions<InsideDbContext> DbContextOptions { get; }
        public static string ConnectionString { get; }

    }
}
