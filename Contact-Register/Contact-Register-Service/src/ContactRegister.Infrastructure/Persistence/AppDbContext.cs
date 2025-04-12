using ContactRegister.Domain.Entities;
using ContactRegister.Domain.Entities.Abstractions;
using ContactRegister.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ContactRegister.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Ddd> Ddds { get; set; }
    
    public AppDbContext() { }
    
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

	public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity.GetType().IsSubclassOfRawGeneric(typeof(AbstractEntity<>)))
            .ToList();

        foreach (var entry in entries)
        {
            if (entry.Entity is IAbstractEntity entity) 
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("CreatedAt").IsModified = false; 
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}