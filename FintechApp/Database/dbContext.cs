using Microsoft.EntityFrameworkCore;


class AppDbContext : DbContext
{
    public required DbSet<Transaction> Transactions {get;set;}
     public required DbSet<User> Users {get;set;}
      public required DbSet<TransactionArchive> TransactionArchives {get;set;}

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Transaction && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (Transaction)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.CreatedDate = DateTime.UtcNow; // Set created date only on insert
            }
            entity.UpdatedDate = DateTime.UtcNow; // Set updated date on both insert and update
        }

        var transactionArchive = ChangeTracker.Entries()
            .Where(e => e.Entity is TransactionArchive && 
                       (e.State == EntityState.Added));

        foreach (var entry in transactionArchive)
        {
            var entity = (TransactionArchive)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.ArchivedAt = DateTime.UtcNow; // Set created date only on insert
            }
            
        }

        return base.SaveChanges();
    }
}