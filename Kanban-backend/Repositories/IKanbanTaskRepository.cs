using Kanban_backend.DTOs;
using Kanban_backend.Models;

namespace Kanban_backend.Repositories
{
    public interface IKanbanTaskRepository
    {
        Task<IEnumerable<KanbanTask>> GetAllByColumnIdAsync(int columnId);
        Task<KanbanTask?> GetKanbanTaskByIdAsync(int id);
        Task<KanbanTask> CreateKanbanTaskAsync(KanbanTask kanbanTask);
        Task<KanbanTask?> UpdateKanbanTaskAsync(KanbanTask kanbanTask);
        Task<KanbanTask?> DeleteKanbanTaskAsync(int id);
        Task<List<KanbanTask>> Reorder(int columnId, ReorderDto reorderDtos);
    }
}
