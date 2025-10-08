using Forager.Core.Board;
using Microsoft.AspNetCore.Mvc;

namespace YourGame.WebApi.Controllers {
    [Route("api/[controller]")]
    public class GameController : ControllerBase {
        // In-memory storage for demo - use database for production
        private static readonly Dictionary<string, GameState> _activeGames = [];
        private static readonly Dictionary<string, string> _playerNames = [];

        public record NewGameRequest(int FieldSize, int NumShrooms, string PlayerName);

        [HttpPost("new")]
        public IActionResult CreateNewGame([FromBody] NewGameRequest request) {
            try {
                var gameId = Guid.NewGuid().ToString();
                var gameState = new GameState(request.FieldSize, request.NumShrooms);

                _activeGames[gameId] = gameState;
                _playerNames[gameId] = request.PlayerName;

                return Ok(new {
                    GameId = gameId,
                    State = GetGameStateResponse(gameState)
                });

            } catch (Exception ex) {
                return BadRequest($"Error creating game: {ex.Message}");
            }
        }

        public record SelectCellRequest(int Row, int Col);

        [HttpPost("{gameId}/action")]
        public IActionResult SelectCell(string gameId, [FromBody] SelectCellRequest request) {
            if (!_activeGames.TryGetValue(gameId, out GameState? gameState))
                return NotFound();

            try {
                // Call your existing game logic
                gameState.Move(request.Row, request.Col);

                return Ok(new {
                    State = GetGameStateResponse(gameState)
                });
            } catch (Exception ex) {
                return BadRequest($"Error processing action: {ex.Message}");
            }
        }

        [HttpDelete("{gameId}")]
        public IActionResult EndGame(string gameId) {
            if (_activeGames.ContainsKey(gameId)) {
                _activeGames.Remove(gameId);
                return Ok(new { Message = "Game ended" });
            }
            return NotFound();
        }

        private static object GetGameStateResponse(GameState gameState) {
            // Convert your game state to a JSON-friendly format
            return new {
                gameState.Cells,
                gameState.NumShroomsFound,
                gameState.CurrentDistance,
                gameState.TargetDistance,
                gameState.IsFinished
            };
        }
    }
}