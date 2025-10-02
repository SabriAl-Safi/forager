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
                    Message = "New game created",
                    State = GetGameStateResponse(gameState)
                });

            } catch (Exception ex) {
                return BadRequest($"Error creating game: {ex.Message}");
            }
        }

        [HttpPost("{gameId}/action")]
        public IActionResult MakeMove(string gameId, [FromBody] GameActionRequest request) {
            if (!_activeGames.ContainsKey(gameId)) {
                return NotFound();
            }

            try {
                var gameState = _activeGames[gameId];

                // Call your existing game logic
                // var result = gameState.ProcessAction(request.ActionType, request.Parameters);

                return Ok(new {
                    //Success = result.Success,
                    //Message = result.Message,
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

        private object GetGameStateResponse(GameState gameState) {
            // Convert your game state to a JSON-friendly format
            return new {
                gameState.Cells,
                gameState.NumShroomsFound,
                gameState.CurrentDistance,
                gameState.GoalDistance
            };
        }
    }

    // Request models
    public class GameActionRequest {
        public string ActionType { get; set; } // e.g., "move", "attack", "buy", etc.
        public Dictionary<string, object> Parameters { get; set; } // Action-specific data
    }
}