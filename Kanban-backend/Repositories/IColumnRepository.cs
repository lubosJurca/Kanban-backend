using Kanban_backend.DTOs;
using Kanban_backend.Models;

namespace Kanban_backend.Repositories
{
    public interface IColumnRepository
    {
        Task<IEnumerable<Column>> GetAllByBoardIdAsync(int boardId);
        Task<Column?> GetColumnByIdAsync(int id);
        Task<Column> CreateColumnAsync(Column column);
        Task<Column?> UpdateColumnAsync(Column column);
        Task<Column?> DeleteColumnAsync(int id);
        Task<List<Column>>  Reorder(int boardId, ReorderDto reorderDtos);
    }
}
