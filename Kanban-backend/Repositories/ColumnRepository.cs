using Kanban_backend.Data;
using Kanban_backend.DTOs;
using Kanban_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Kanban_backend.Repositories
{
    public class ColumnRepository : IColumnRepository
    {
        private readonly AppDbContext _columnContext;

        public ColumnRepository(AppDbContext context)
        {
            _columnContext = context;
        }

        // ------------------------------- GET ALL COLUMNS BY BOARD ID -------------------------------
        public async Task<IEnumerable<Column>> GetAllByBoardIdAsync(int boardId)
        {
            return await _columnContext.Columns
                .Where(c => c.BoardId == boardId && c.Board != null)
                .OrderBy(c => c.Order)
                .ToListAsync();
        }

        // ------------------------------- GET COLUMN BY ID -------------------------------
        public async Task<Column?> GetColumnByIdAsync(int id)
        {
            return await _columnContext.Columns.FirstOrDefaultAsync(c => c.Id == id);
        }

        // ------------------------------- CREATE COLUMN -------------------------------
        public async Task<Column> CreateColumnAsync(Column column)
        {
            var maxOrder = await _columnContext.Columns
                .Where(c => c.BoardId == column.BoardId)
                .MaxAsync(c => (int?)c.Order) ?? 0; // Get the highest order value or 0 if none exist

            column.Order = maxOrder + 1;
            _columnContext.Columns.Add(column); // No need for await here because we just need to add the object to the EF Core change tracker
            await _columnContext.SaveChangesAsync(); // This is where the actual database operation happens
            return column;
        }

        // ------------------------------- UPDATE COLUMN -------------------------------
        public async Task<Column?> UpdateColumnAsync(Column column)
        {
            column.UpdatedAt = DateTime.UtcNow;
            await _columnContext.SaveChangesAsync();
            return column;
        }


        // ------------------------------- DELETE COLUMN -------------------------------
        public async Task<Column?> DeleteColumnAsync(int id)
        {
            var columnToDelete = await _columnContext.Columns.FirstOrDefaultAsync(c => c.Id == id);
            if (columnToDelete == null) return null;
            _columnContext.Columns.Remove(columnToDelete);
            await _columnContext.SaveChangesAsync();
            return columnToDelete;
        }

        // ------------------------------- REORDER COLUMNS -------------------------------
        public async Task<List<Column>> Reorder(int boardId, ReorderDto reorderDtos)
        {
            var columns = await _columnContext.Columns
                .Where(c => c.BoardId == boardId)
                .ToListAsync();

            for (int i = 0; i < reorderDtos.Ids.Count; i++)
            {
                var column = columns.First(c => c.Id == reorderDtos.Ids[i]);
                column.Order = i + 1;
            }

            await _columnContext.SaveChangesAsync();
            
            return columns;

        }

    }
}
