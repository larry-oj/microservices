using Microsoft.EntityFrameworkCore;

namespace KnowYourPostUsers.Data;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public UserContext(DbContextOptions<UserContext> options) 
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(e => e.Email)
            .IsUnique();
    }
}