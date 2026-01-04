using Kanban_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Kanban_backend.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Board> Boards => Set<Board>();
        public DbSet<Column> Columns => Set<Column>();
        public DbSet<KanbanTask> KanbanTasks => Set<KanbanTask>();
        public DbSet<Subtask> Subtasks => Set<Subtask>();
        public DbSet<User> Users => Set<User>();
    }
}
