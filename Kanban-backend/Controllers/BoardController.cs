using Kanban_backend.DTOs;
using Kanban_backend.Models;
using Kanban_backend.Repositories;
using Kanban_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kanban_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BoardController : ControllerBase
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IAuthService _authorizationService;
        public BoardController(IBoardRepository boardRepository, IAuthService authorizationService)
        {
            _boardRepository = boardRepository;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardDto>>> GetAllBoards() {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Thanks to the Authorize attribute, we can safely assume that the user is authenticated and has a NameIdentifier claim

            try
            {
                var boards = await _boardRepository.GetAllByUserAsync(userId);

                var boardDtos = boards.Select(board => new BoardDto
                {
                    Id = board.Id,
                    Title = board.Title,
                    UserId = board.UserId,
                    CreatedAt = board.CreatedAt,
                    UpdatedAt = board.UpdatedAt
                });

                return Ok(boardDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {message = "An error occurred"});
            }

            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDto>> GetBoardById(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var hasAccess = await _authorizationService.HasAccessToBoard(userId, id);

            if (!hasAccess) return NotFound();

            var board = await _boardRepository.GetBoardByIdAsync(id);

            var boardDto = new BoardDto
            {
                Id = board.Id,
                Title = board.Title,
                UserId = board.UserId,
                CreatedAt = board.CreatedAt,
                UpdatedAt = board.UpdatedAt
            };

            return Ok(boardDto);
        }

        [HttpPost]
        public async Task<ActionResult<BoardDto>> CreateBoard(CreateBoardDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var board = new Board
            {
                Title = dto.Title,
                UserId = userId

            }; 
            var boardToCreate = await _boardRepository.CreateBoardAsync(board);

            var boardDto = new BoardDto
            {
                Id = boardToCreate.Id,
                Title = boardToCreate.Title,
                UserId = boardToCreate.UserId,
                CreatedAt = boardToCreate.CreatedAt,
                UpdatedAt = boardToCreate.UpdatedAt

            };

            return CreatedAtAction(nameof(GetBoardById), new { id = boardDto.Id }, boardDto);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BoardDto>> UpdateBoard(int id,UpdateBoardDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var hasAccess = await _authorizationService.HasAccessToBoard(userId, id);

            if (!hasAccess) return NotFound();

            var boardToUpdate = await _boardRepository.GetBoardByIdAsync(id);

            boardToUpdate.Title = dto.Title;

            var updatedBoard = await _boardRepository.UpdateBoardAsync(boardToUpdate);

            var updatedBoardDto = new BoardDto
            {
                Id = updatedBoard!.Id,
                Title = updatedBoard.Title,
                UserId = updatedBoard.UserId,
                CreatedAt = updatedBoard.CreatedAt,
                UpdatedAt = updatedBoard.UpdatedAt
            };

            return Ok(updatedBoardDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BoardDto>> DeleteBoardById(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var hasAccess = await _authorizationService.HasAccessToBoard(userId, id);

            if (!hasAccess) return NotFound();

            var deletedBoard = await _boardRepository.DeleteBoardAsync(id);

            var deletedBoardDto = new BoardDto
            {
                Id = deletedBoard.Id,
                Title = deletedBoard.Title,
                UserId = deletedBoard.UserId,
                CreatedAt = deletedBoard.CreatedAt,
                UpdatedAt = deletedBoard.UpdatedAt
            };
            return Ok(deletedBoardDto);
        }

    }
}
