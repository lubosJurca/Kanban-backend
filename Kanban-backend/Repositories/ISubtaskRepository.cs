using Kanban_backend.DTOs;
using Kanban_backend.Models;

namespace Kanban_backend.Repositories
{
    public interface ISubtaskRepository
    {
        Task<IEnumerable<Subtask>> GetAllByKanbanTaskIdAsync(int kanbanTaskId);
        Task<Subtask?> GetSubtaskByIdAsync(int id);
        Task<Subtask> CreateSubtaskAsync(Subtask subtask);
        Task<Subtask?> UpdateSubtaskAsync(Subtask subtask);
        Task<Subtask?> DeleteSubtaskAsync(int id);
        Task<IEnumerable<Subtask>> Reorder(int kanbanTaskId, ReorderDto reorderDtos);
    }
}
