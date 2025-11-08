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
        private readonly int numStoneWalls;

        private static readonly HashSet<CellState> _shroomPhases = [
            CellState.Start, CellState.Hole, CellState.Forager
        ];

        private Cell _start = null;
        private Cell _lastClicked = null;
        private int _tourDistance = 0;
        private List<Cell> _lastChangedCells = [];
        private bool _isFinished = false;

        public int NumShroomsFound => _tourCells.Count;
        public int CurrentDistance => _tourDistance;
        public int TargetDistance => _targetDistance;
        public List<Cell> Cells => _lastChangedCells;
        public IEnumerable<Cell> AllCells => _cells.SelectMany(c => c);
        public bool IsFinished => _isFinished;
        public Cell GetCell(int row, int col) => _cells[row][col];

        public GameState(int fieldSize, int numShrooms, int pcStone) {
            _fieldSize = fieldSize;
            _numShrooms = numShrooms;
            _cells = new Cell[_fieldSize][];
            numStoneWalls = pcStone * fieldSize * fieldSize / 100;

            for (int i = 0; i < _fieldSize; i++) {
                _cells[i] = new Cell[_fieldSize];
                for (int j = 0; j < _fieldSize; j++) {
                    var cell = new Cell { Row = i, Col = j, State = CellState.Grass };
                    _cells[i][j] = cell;
                }
            }

            var rnd = new Random();
            SpawnStoneWall(rnd);
            _shroomCells = SpawnShrooms(rnd);

            var matrix = GetDistanceMatrix();
            var tsp = new TSP(matrix);
            var tour = tsp.Solve();
            _targetDistance = tour.Cost;
            _lastChangedCells = [.. AllCells];
        }

        private void SpawnStoneWall(Random rnd) {
            var num = 0;
            while (num < numStoneWalls) {
                var iM = rnd.Next(0, _fieldSize);
                var jM = rnd.Next(0, _fieldSize);
                var cell = _cells[iM][jM];
                if (cell.IsShroom || cell.State == CellState.Stone)
                    continue;

                cell.State = CellState.Stone;
                cell.Ctr = num;
                num++;
            }
        }

        private Cell[] SpawnShrooms(Random rnd) {
            var shroomCells = new Cell[_numShrooms];
            var num = 0;
            while (num < _numShrooms) {
                var iM = rnd.Next(0, _fieldSize);
                var jM = rnd.Next(0, _fieldSize);
                var cell = _cells[iM][jM];
                if (cell.IsShroom || cell.State == CellState.Stone)
                    continue;

                cell.State = CellState.Shroom;
                cell.Ctr = num;
                shroomCells[num] = cell;
                num++;
            }
            return shroomCells;
        }

        private int[][] GetDistanceMatrix() {
            var matrix = new int[_numShrooms][];
            for (int i = 0; i < _numShrooms; i++) {
                var source = _shroomCells[i];
                matrix[i] = new int[_numShrooms];
                for (int j = 0; j < _numShrooms; j++) {
                    matrix[i][j] = source.DistanceTo(_shroomCells[j]);
                }
            }
            return matrix;
        }

        public void Move(int toRow, int toCol) {
            _lastChangedCells = [];
            foreach (var cell in AllCells) {
                if (!cell.IsTrodden) continue;
                cell.IsTrodden = false;
                _lastChangedCells.Add(cell);
            }

            var toCell = _cells[toRow][toCol];
            toCell.State = CellState.Forager;
            _lastChangedCells.Add(toCell);
            _tourCells.Add(toCell);
            if (_lastClicked != null) {
                _lastClicked.State = _lastClicked == _start ? CellState.Start : CellState.Hole;
                _tourDistance += toCell.DistanceTo(_lastClicked);
                var route = RouteTo(_lastClicked, toCell);
                foreach (var routeCell in route) {
                    routeCell.IsTrodden = true;
                    _lastChangedCells.Add(routeCell);
                }
                _isFinished = (toCell == _start);
            } else {
                _start = toCell;
            }
            _lastClicked = toCell;
        }

        public List<Cell> Reset() {
            _start = null;
            _lastClicked = null;
            _isFinished = false;
            _tourDistance = 0;
            _lastChangedCells = [];
            foreach (var cell in AllCells) {
                var isChanged = false;
                if (cell.IsTrodden) {
                    cell.IsTrodden = false;
                    isChanged = true;
                }

                if (_shroomPhases.Contains(cell.State)) {
                    cell.State = CellState.Shroom;
                    isChanged = true;
                }

                if (isChanged) {
                    _lastChangedCells.Add(cell);
                }
            }
            return _lastChangedCells;
        }

        public List<Cell> RouteTo(Cell from, Cell to) {
            var rowRange = IntRange(from.Row, to.Row);
            var colRange = IntRange(from.Col, to.Col);

            if (from.Col == to.Col)
                return [.. rowRange.Select(r => _cells[r][from.Col])];

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

        private static IEnumerable<int> IntRange(int start, int end) =>
            start < end ?
            Enumerable.Range(start, 1 + end - start) :
            Enumerable.Range(end, 1 + start - end).Reverse();
    }
}
