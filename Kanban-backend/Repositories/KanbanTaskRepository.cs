using Kanban_backend.Data;
using Kanban_backend.DTOs;
using Kanban_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Kanban_backend.Repositories
{
    public class KanbanTaskRepository : IKanbanTaskRepository
    {
        private readonly AppDbContext _kanbanTaskContext;

        public KanbanTaskRepository(AppDbContext context)
        {
            _kanbanTaskContext = context;
        }

        // -------------------- GET ALL BY COLUMN ID --------------------
        public async Task<IEnumerable<KanbanTask>> GetAllByColumnIdAsync(int columnId)
        {
            return await _kanbanTaskContext.KanbanTasks
                .Where(kt => kt.ColumnId == columnId)
                .OrderBy(kt => kt.Order)
                .ToListAsync();
        }

        // -------------------- GET KANBAN TASK BY ID --------------------
        public async Task<KanbanTask?> GetKanbanTaskByIdAsync(int id)
        {
            return await _kanbanTaskContext.KanbanTasks
                .FirstOrDefaultAsync(kt => kt.Id == id);
        }

        // -------------------- CREATE KANBAN TASK --------------------
        public async Task<KanbanTask> CreateKanbanTaskAsync(KanbanTask kanbanTask)
        {
            var maxOrder = await _kanbanTaskContext.KanbanTasks
                .Where(kt => kt.ColumnId == kanbanTask.ColumnId)
                .MaxAsync(kt => (int?)kt.Order) ?? 0; // Get the highest order value or 0 if none exist

            kanbanTask.Order = maxOrder + 1; // Set the new task's order to be the highest + 1
            _kanbanTaskContext.KanbanTasks.Add(kanbanTask);
            await _kanbanTaskContext.SaveChangesAsync();
            return kanbanTask; 
        }

        // -------------------- UPDATE KANBAN TASK --------------------
        public async Task<KanbanTask?> UpdateKanbanTaskAsync(KanbanTask kanbanTask)
        {
            await _kanbanTaskContext.SaveChangesAsync();
            return kanbanTask;
        }

        // -------------------- DELETE KANBAN TASK --------------------
        public async Task<KanbanTask?> DeleteKanbanTaskAsync(int id)
        {
            var kanbanTaskToDelete = await _kanbanTaskContext.KanbanTasks.FirstOrDefaultAsync(kt => kt.Id == id);
            if (kanbanTaskToDelete == null) return null;

            _kanbanTaskContext.KanbanTasks.Remove(kanbanTaskToDelete);
            await _kanbanTaskContext.SaveChangesAsync();
            return kanbanTaskToDelete;
        }

        // -------------------- REORDER KANBAN TASKS --------------------
        public async Task<List<KanbanTask>> Reorder(int columnId, ReorderDto reorderDtos)
        {
            var kanbanTasks = await _kanbanTaskContext.KanbanTasks
                .Where(kt => kt.ColumnId == columnId)
                .ToListAsync();

            for (int i = 0; i < reorderDtos.Ids.Count; i++)
            {
                var kanbanTask = kanbanTasks.First(c => c.Id == reorderDtos.Ids[i]);
                kanbanTask.Order = i + 1;
            }

            await _kanbanTaskContext.SaveChangesAsync();
            return kanbanTasks.OrderBy(t => t.Order).ToList();
        }

       
    }
}
