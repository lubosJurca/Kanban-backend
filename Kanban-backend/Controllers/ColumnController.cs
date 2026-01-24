using Kanban_backend.Repositories;
using Kanban_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Kanban_backend.DTOs;

namespace Kanban_backend.Controllers
{
    [Route("api/board/{boardId}/column")]
    [ApiController]
    public class ColumnController : ControllerBase
    {
        private readonly IColumnRepository _columnRepository;
        private readonly IAuthorizationService _authorizationService;
        public ColumnController(IColumnRepository columnRepository, IAuthorizationService authorizationService)
        {
            _columnRepository = columnRepository;
            _authorizationService = authorizationService;
        }

        // ------------------------------- GET ALL COLUMNS BY BOARD ID -------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColumnDto>>> GetAllColumns(int boardId)
        {
            var userId = 1; // Temporary hardcoded user ID
            var hasAccess = await _authorizationService.HasAccessToBoard(userId, boardId);

            if (!hasAccess) return NotFound(); // Return 404 to avoid leaking information about the existence of the board

            var columns = await _columnRepository.GetAllByBoardIdAsync(boardId);

            var columnDtos = columns.Select(c => new ColumnDto
            {
                Id = c.Id,
                BoardId = c.BoardId,
                Title = c.Title,
                Order = c.Order,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();

            return Ok(columnDtos);
        }

        // ------------------------------- GET SINGLE COLUMN BY COLUMN ID -------------------------------
        [HttpGet]
        [Route("/api/column/{columnId}", Name = "GetSingleColumn")] // No need for boardId here
        public async Task<ActionResult<ColumnDto>> GetSingleColumnAsync(int columnId)
        {
            var userId = 1; // Temporary hardcoded user ID
            var hasAccess = await _authorizationService.HasAccessToColumn(userId, columnId);

            if (!hasAccess) return NotFound(); // Return 404 to avoid leaking information about the existence of the column

            var column = await _columnRepository.GetColumnByIdAsync(columnId);

            //No need to check for null since HasAccessToColumn would return false if the column doesn't exist

            var columnDto = new ColumnDto
            {
                Id = column.Id,
                BoardId = column.BoardId,
                Title = column.Title,
                Order = column.Order,
                CreatedAt = column.CreatedAt,
                UpdatedAt = column.UpdatedAt
            };

            return Ok(columnDto);

        }

        // ------------------------------- CREATE COLUMN -------------------------------
        [HttpPost]
        public async Task<ActionResult<ColumnDto>> CreateColumnAsync(CreateColumnDto columnDto,int boardId) 
        { 
            var userId = 1;
            var hasAccess = await _authorizationService.HasAccessToBoard(userId, boardId);

            if (!hasAccess) return NotFound();

            var column = new Models.Column
            {
                BoardId = boardId,
                Title = columnDto.Title,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdColumn = await _columnRepository.CreateColumnAsync(column);

            var createdColumnDto = new ColumnDto
            {
                Id = createdColumn.Id,
                BoardId = createdColumn.BoardId,
                Title = createdColumn.Title,
                Order = createdColumn.Order,
                CreatedAt = createdColumn.CreatedAt,
                UpdatedAt = createdColumn.UpdatedAt
            };
            return CreatedAtRoute("GetSingleColumn", new { columnId = createdColumn.Id }, createdColumnDto);
        }

        // ------------------------------- UPDATE COLUMN -------------------------------
        [HttpPut]
        [Route("/api/column/{columnId}")]
        public async Task<ActionResult<ColumnDto>> UpdateColumnAsync(int columnId, UpdateColumnDto updateColumnDto)
        {
            var userId = 1; // Temporary hardcoded user ID
            var hasAccess = await _authorizationService.HasAccessToColumn(userId, columnId);

            if (!hasAccess) return NotFound(); // Return 404 to avoid leaking information about the existence of the column

            var column = await _columnRepository.GetColumnByIdAsync(columnId);
            column.Title = updateColumnDto.Title;

            var updatedColumn = await _columnRepository.UpdateColumnAsync(column);

            var updatedColumnDto = new ColumnDto
            {
                Id = updatedColumn.Id,
                BoardId = updatedColumn.BoardId,
                Title = updatedColumn.Title,
                Order = updatedColumn.Order,
                CreatedAt = updatedColumn.CreatedAt,
                UpdatedAt = updatedColumn.UpdatedAt
            };

            return Ok(updateColumnDto);
        }

        // ------------------------------- DELETE COLUMN -------------------------------
        [HttpDelete]
        [Route("/api/column/{columnId}")]
        public async Task<ActionResult<ColumnDto>> DeleteColumnAsync(int columnId)
        {
            var userId = 1; // Temporary hardcoded user ID
            var hasAccess = await _authorizationService.HasAccessToColumn(userId, columnId);

            if (!hasAccess) return NotFound();

            var deletedColumn = await _columnRepository.DeleteColumnAsync(columnId);

            var deletedColumnDto = new ColumnDto
            {
                Id = deletedColumn.Id,
                BoardId = deletedColumn.BoardId,
                Title = deletedColumn.Title,
                Order = deletedColumn.Order,
                CreatedAt = deletedColumn.CreatedAt,
                UpdatedAt = deletedColumn.UpdatedAt
            };

            return Ok(deletedColumnDto);
            
        }

        // ------------------------------- REORDER COLUMNS -------------------------------
        [HttpPut("reorder")]
        public async Task<ActionResult<IEnumerable<ColumnDto>>> ReorderColumnsByIdsAsync(int boardId, ReorderDto reorderDtos)
        {
            var userId = 1;
            var hasAccess = await _authorizationService.HasAccessToBoard(userId, boardId);

            if (!hasAccess) return NotFound();

            var reorderedColumns = await _columnRepository.Reorder(boardId, reorderDtos);

            var reordereColumnsDto = reorderedColumns.Select(c => new ColumnDto
            {
                Id = c.Id,
                BoardId = c.BoardId,
                Title = c.Title,
                Order = c.Order,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();

            return Ok(reordereColumnsDto);
        }
    }
}
