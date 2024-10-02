using Microsoft.EntityFrameworkCore;

namespace TaskMinApi;

public class DataContext : DbContext
{
    // Constructor that passes options to the base DbContext class
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    // Method to configure the database connection
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        // Configure the DbContext to use a SQL Server database
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=TaskManagementDB;Trusted_Connection=true;TrustServerCertificate=true;");
        // Connection string specifies the local SQL Server instance and database name
    }

    // DbSet property to represent the Tasks table in the database
    public DbSet<Task> Tasks => Set<Task>();
}
