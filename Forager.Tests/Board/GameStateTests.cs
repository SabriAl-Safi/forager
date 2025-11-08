using Forager.Core;
using Forager.Core.Board;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forager.Tests.Board {
    [TestFixture]
    public class GameStateTests {
        private int _fieldSize = 10;
        private int _numShrooms = 10;
        private GameState _gameState { get; set; }

        private void BuildGameState() => _gameState = new GameState(_fieldSize, _numShrooms, 0);

        [Test]
        public void RouteTo() {
            BuildGameState();
            List<Cell> expected;
            List<Cell> actual;

            actual = _gameState.RouteTo(_gameState.GetCell(0, 0), _gameState.GetCell(2, 2));
            expected = GetCellsByCoord((0, 0), (0, 1), (0, 2), (1, 2), (2, 2));
            Assert.That(actual, Is.EqualTo(expected));

            actual = _gameState.RouteTo(_gameState.GetCell(0, 0), _gameState.GetCell(0, 2));
            expected = GetCellsByCoord((0, 0), (0, 1), (0, 2));
            Assert.That(actual, Is.EqualTo(expected));

            actual = _gameState.RouteTo(_gameState.GetCell(0, 0), _gameState.GetCell(2, 0));
            expected = GetCellsByCoord((0, 0), (1, 0), (2, 0));
            Assert.That(actual, Is.EqualTo(expected));

            actual = _gameState.RouteTo(_gameState.GetCell(0, 2), _gameState.GetCell(2, 0));
            expected = GetCellsByCoord((0, 2), (1, 2), (2, 2), (2, 1), (2, 0));
            Assert.That(actual, Is.EqualTo(expected));
        }

        private List<Cell> GetCellsByCoord(params (int row, int col)[] values) =>
            [.. values.Select(v => _gameState.GetCell(v.row, v.col))];
    }
}
