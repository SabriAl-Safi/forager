using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forager.Core.Board {
    public class GameState {
        private readonly int _fieldSize;
        private readonly int _numShrooms;
        private readonly Cell[][] _cells;
        private readonly HashSet<Cell> _tourCells = [];
        private readonly Cell[] _shroomCells;
        private readonly int _targetDistance;
        //private readonly Cell _start;

        private Cell _lastClicked = null;
        private int _tourDistance = 0;

        public int TurnNumber = 9;
        public string CurrentPlayer = "Sabri";
        public string GameStatus = "In progress";

        public int NumShroomsFound => _tourCells.Count;
        public int CurrentDistance => _tourDistance;
        public int TargetDistance => _targetDistance;

        public Cell[] Cells => [.._cells.SelectMany(c => c)];

        public GameState(int fieldSize, int numShrooms) {
            _fieldSize = fieldSize;
            _numShrooms = numShrooms;
            _cells = new Cell[_fieldSize][];
            for (int i = 0; i < _fieldSize; i++) {
                _cells[i] = new Cell[_fieldSize];
                for (int j = 0; j < _fieldSize; j++) {
                    var cell = new Cell { Row = i, Col = j, State = CellState.Grass };
                    _cells[i][j] = cell;
                }
            }

            var rnd = new Random();
            _shroomCells = new Cell[_numShrooms];
            var num = 0;
            while (num < _numShrooms) {
                var iM = rnd.Next(0, _fieldSize);
                var jM = rnd.Next(0, _fieldSize);
                var cell = _cells[iM][jM];
                if (cell.IsShroom)
                    continue;

                cell.State = CellState.Shroom;
                _shroomCells[num] = cell;
                num++;
            }

            var matrix = new int[_numShrooms][];
            for (int i = 0; i < _numShrooms; i++) {
                var source = _shroomCells[i];
                matrix[i] = new int[_numShrooms];
                for (int j = 0; j < _numShrooms; j++) {
                    matrix[i][j] = source.DistanceTo(_shroomCells[j]);
                }
            }

            var tsp = new TSP(matrix);
            var tour = tsp.Solve();
            _targetDistance = tour.Cost;
        }

        public void Move(int row, int col) {
            var cell = _cells[row][col];
            cell.State = CellState.Forager;
            _tourCells.Add(cell);
            if (_lastClicked != null) {
                _lastClicked.State = CellState.Hole;
                _tourDistance += cell.DistanceTo(_lastClicked);
            }
            _lastClicked = cell;
        }

        public void Reset() {
        }

        public void Select() {
        }
    }
}
