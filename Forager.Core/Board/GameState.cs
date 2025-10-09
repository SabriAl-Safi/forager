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

        private Cell _start = null;
        private Cell _lastClicked = null;
        private int _tourDistance = 0;
        private List<Cell> _lastChangedCells = [];
        private bool _isFinished = false;

        public int NumShroomsFound => _tourCells.Count;
        public int CurrentDistance => _tourDistance;
        public int TargetDistance => _targetDistance;
        public List<Cell> Cells => _lastChangedCells;
        public bool IsFinished => _isFinished;
        public Cell GetCell(int row, int col) => _cells[row][col];

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
                cell.Ctr = num;
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
            _lastChangedCells = [.. _cells.SelectMany(c => c)];
        }

        public void Move(int row, int col) {
            _lastChangedCells = [];
            foreach (var lcc in _cells.SelectMany(c => c)) {
                if (!lcc.IsTrodden) continue;
                lcc.IsTrodden = false;
                _lastChangedCells.Add(lcc);
            }

            var cell = _cells[row][col];
            cell.State = CellState.Forager;
            _lastChangedCells.Add(cell);
            _tourCells.Add(cell);
            if (_lastClicked != null) {
                _lastClicked.State = _lastClicked == _start ? CellState.Start : CellState.Hole;
                _tourDistance += cell.DistanceTo(_lastClicked);
                var route = RouteTo(_lastClicked, cell);
                foreach (var routeCell in route) {
                    routeCell.IsTrodden = true;
                    _lastChangedCells.Add(routeCell);
                }
                _isFinished = (cell == _start);
            } else {
                _start = cell;
            }
            _lastClicked = cell;
        }

        public void Reset() {
        }

        public List<Cell> RouteTo(Cell from, Cell to) {
            var rowRange = IntRange(from.Row, to.Row);
            var colRange = IntRange(from.Col, to.Col);

            if (from.Col == to.Col)
                return [..rowRange.Select(r => _cells[r][from.Col])];

            if (from.Row == to.Row)
                return [.. colRange.Select(c => _cells[from.Row][c])];

            if ((from.Col < to.Col) != (from.Row < to.Row)) {
                return rowRange
                    .Select(r => _cells[r][from.Col])
                    .Concat(colRange.Select(c => _cells[to.Row][c]).Skip(1))
                    .ToList();
            }

            return colRange
                .Select(c => _cells[from.Row][c])
                .Concat(rowRange.Select(r => _cells[r][to.Col]).Skip(1))
                .ToList();
        }

        private IEnumerable<int> IntRange(int start, int end) =>
            start < end ?
            Enumerable.Range(start, 1 + end - start) :
            Enumerable.Range(end, 1 + start - end).Reverse();
    }
}
