using Kanban_backend.DTOs;
using Kanban_backend.Models;
using Kanban_backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kanban_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardRepository _boardRepository;
        public BoardController(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardDto>>> GetAllBoards() {
            int userId = 1; // Temporary hardcoded user ID

            //TODO: Replace with actual user ID from authentication context

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

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDto>> GetBoardById(int id)
        {
            var board = await _boardRepository.GetBoardByIdAsync(id);

            if (board == null) return NotFound();

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
            var board = new Board
            {
                Title = dto.Title,
                UserId = 1 // Temporary hardcoded user ID
                
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

            //TODO: Replace with actual user ID from authentication context
            return CreatedAtAction(nameof(GetBoardById), new { id = boardDto.Id }, boardDto);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BoardDto>> UpdateBoard(int id,UpdateBoardDto dto)
        {

            var boardToUpdate = await _boardRepository.GetBoardByIdAsync(id);

            if (boardToUpdate == null)
            {
                return NotFound($"Board with id {id} not found");
            }

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
            var deletedBoard = await _boardRepository.DeleteBoardAsync(id);

            if (deletedBoard == null) return NotFound();

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
