using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ITSupportBE.Api.Data; // 💡 hoặc ITSupportBE.Api.Models nếu DbContext bạn đặt ở đó

namespace ITSupportBE.Api.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ITSupportDbContext>
    {
        public ITSupportDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine(">>> DesignTimeDbContextFactory đang được gọi <<<");

            var optionsBuilder = new DbContextOptionsBuilder<ITSupportDbContext>();

            // ✅ Connection string SQL Server
            var connectionString =
                "Server=TUANDACA\\SQLEXPRESS;Database=ITSupportDB;Trusted_Connection=True;TrustServerCertificate=True;";

            // ✅ Dùng SQL Server provider
            optionsBuilder.UseSqlServer(connectionString);

            return new ITSupportDbContext(optionsBuilder.Options);
        }
    }
}
