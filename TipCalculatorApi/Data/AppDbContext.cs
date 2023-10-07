using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Bill> Bills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(@"Server=localhost;Database=TipCalculator;Trusted_Connection=True;");
}