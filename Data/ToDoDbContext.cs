using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data;

public class ToDoDbContext : DbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the Item entity
        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Items");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(e => e.IsComplete)
                .IsRequired();

            // Add indexes for better performance
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.IsComplete);
        });

        // Optional: Seed some initial data
        modelBuilder.Entity<Item>().HasData(
            new Item { Id = 1, Name = "Learn ASP.NET Core", IsComplete = false },
            new Item { Id = 2, Name = "Build TodoAPI", IsComplete = false },
            new Item { Id = 3, Name = "Deploy to production", IsComplete = false }
        );
    }
}
