using Kanban_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kanban_backend.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>,int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Board> Boards => Set<Board>();
        public DbSet<Column> Columns => Set<Column>();
        public DbSet<KanbanTask> KanbanTasks => Set<KanbanTask>();
        public DbSet<Subtask> Subtasks => Set<Subtask>();


        //This block of code is responsible for automatically setting the CreatedAt and UpdatedAt timestamps for entities that implement the ITimestampedEntity interface whenever changes are saved to the database.
        // Override SaveChanges and SaveChangesAsync to update timestamps
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        // Override SaveChangesAsync to update timestamps
        //cancellationToken is cool feature that allows you to cancel the async operation if needed, for example if the request is aborted. It helps to free up resources and improve performance in certain scenarios.
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            // Get all entries that implement ITimestampedEntity and are being Added or Modified
            var timestampedEntries = ChangeTracker.Entries<ITimestampedEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            // Update timestamps accordingly
            foreach (var entry in timestampedEntries)
            {
                // For Added entities, set both CreatedAt and UpdatedAt
                if (entry.State == EntityState.Added)
                {
                  
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                // For Modified entities, only update UpdatedAt
                else if (entry.State == EntityState.Modified)
                {
                
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

        }
        }
}
