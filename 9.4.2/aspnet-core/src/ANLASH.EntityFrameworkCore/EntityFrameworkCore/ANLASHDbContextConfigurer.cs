using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ANLASH.EntityFrameworkCore
{
    public static class ANLASHDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ANLASHDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ANLASHDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
