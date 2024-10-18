using Microsoft.EntityFrameworkCore;

namespace TaskMinApi;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=TaskManagementDB;Trusted_Connection=true;TrustServerCertificate=true;");
    }

    public DbSet<global::Task> Tasks => Set<global::Task>();
    public DbSet<global::User> Users => Set<global::User>();
}