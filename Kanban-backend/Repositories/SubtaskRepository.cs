using Kanban_backend.Data;
using Kanban_backend.DTOs;
using Kanban_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Kanban_backend.Repositories
{
    public class SubtaskRepository : ISubtaskRepository
    {
        private readonly AppDbContext _subtaskContext;
        public SubtaskRepository(AppDbContext context) 
        {
            _subtaskContext = context;
        }

        // -------------------- GET ALL BY KANBAN TASK ID --------------------
        public async Task<IEnumerable<Subtask>> GetAllByKanbanTaskIdAsync(int kanbanTaskId)
        {
            return await _subtaskContext.Subtasks
                    .Where(sub => sub.KanbanTaskId == kanbanTaskId)
                    .OrderBy(sub => sub.Order)
                    .ToListAsync();
        }

        // -------------------- GET SUBTASK BY ID --------------------
        public async Task<Subtask?> GetSubtaskByIdAsync(int id)
        {
            return await _subtaskContext.Subtasks
                    .FirstOrDefaultAsync(sub => sub.Id == id);
        }

        // -------------------- CREATE SUBTASK --------------------
        public async Task<Subtask> CreateSubtaskAsync(Subtask subtask)
        {
            var maxOrder = await _subtaskContext.Subtasks
                                .Where(sub => sub.KanbanTaskId == subtask.KanbanTaskId)
                                .MaxAsync(sub => (int?)sub.Order) ?? 0;

            subtask.Order = maxOrder + 1;
            _subtaskContext.Subtasks.Add(subtask);
            await _subtaskContext.SaveChangesAsync();
            return subtask;
        }

        // -------------------- UPDATE SUBTASK --------------------
        public async Task<Subtask?> UpdateSubtaskAsync(Subtask subtask)
        {
            await _subtaskContext.SaveChangesAsync();
            return subtask;
        }

        // -------------------- DELETE SUBTASK --------------------
        public async Task<Subtask?> DeleteSubtaskAsync(int id)
        {
            var subtask = await _subtaskContext.Subtasks.FirstOrDefaultAsync(sub => sub.Id == id);
            if(subtask == null) return null;
            _subtaskContext.Subtasks.Remove(subtask);
            await _subtaskContext.SaveChangesAsync();
            return subtask;
        }

        // -------------------- REORDER SUBTASKS --------------------
        public async Task<IEnumerable<Subtask>> Reorder(int kanbanTaskId, ReorderDto reorderDtos)
        {
            var subtasks = await _subtaskContext.Subtasks
                                .Where(sub => sub.KanbanTaskId == kanbanTaskId)
                                .ToListAsync();

            for (int i = 0; i < reorderDtos.Ids.Count; i++)
            {
                var subtask = subtasks.First(c => c.Id == reorderDtos.Ids[i]);
                subtask.Order = i + 1;
            }
            await _subtaskContext.SaveChangesAsync();
            return subtasks;
        }
    }
}
