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
        private readonly int _numStoneWalls;
        private readonly Random _rnd = new();

        private static readonly int[] _deltas = [1, -1];
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
            _numStoneWalls = pcStone * fieldSize * fieldSize / 100;

            SpawnGrass();
            SpawnStoneWall();
            _shroomCells = SpawnShrooms();

            var matrix = GetDistanceMatrix();
            var tsp = new TSP(matrix);
            var tour = tsp.Solve();
            _targetDistance = tour.Cost;
            _lastChangedCells = [.. AllCells];
        }

        private void SpawnGrass() {
            for (int i = 0; i < _fieldSize; i++) {
                _cells[i] = new Cell[_fieldSize];
                for (int j = 0; j < _fieldSize; j++) {
                    var cell = new Cell { Row = i, Col = j, State = CellState.Grass };
                    _cells[i][j] = cell;
                }
            }
        }

        private void SpawnStoneWall() {
            for (int num = 0; num < _numStoneWalls; num++) {
                var cell = GetRandomCell(CellState.Grass);
                cell.State = CellState.Stone;

                var neighbours = GetNeighbours(cell)
                    .Where(c => c.State == CellState.Grass)
                    .ToArray();

                if (neighbours.Length < 2)
                    continue;

                var source = neighbours[0];
                var targets = neighbours[1..].ToHashSet();

                foreach (var bfsCell in BFS(source)) {
                    targets.Remove(bfsCell);
                    if (targets.Count == 0)
                        break;
                }

                if (targets.Count > 0) {
                    cell.State = CellState.Grass;
                    num--;
                }
            }
        }

        private IEnumerable<Cell> GetNeighbours(Cell cell) {
            foreach (var rowDelta in _deltas) {
                var newRow = cell.Row + rowDelta;

                if (!IsInField(newRow, cell.Col))
                    continue;

                yield return _cells[newRow][cell.Col];
            }

            foreach (var colDelta in _deltas) {
                var newCol = cell.Col + colDelta;

                if (!IsInField(cell.Row, newCol))
                    continue;

                yield return _cells[cell.Row][newCol];
            }
        }

        private IEnumerable<Cell> BFS(Cell cell) {
            var queue = new Queue<Cell>();
            var processed = new HashSet<Cell>();
            queue.Enqueue(cell);

            while (queue.Count > 0) {
                var curCell = queue.Dequeue();
                yield return curCell;

                foreach (var nbCell in GetNeighbours(curCell)) {
                    if (nbCell.State == CellState.Stone)
                        continue;

                    if (processed.Contains(nbCell))
                        continue;

                    queue.Enqueue(nbCell);
                }
                processed.Add(curCell);
            }
        }

        private bool IsInField(int row, int col) => row >= 0 && col >= 0 && row < _fieldSize && col < _fieldSize;

        private Cell[] SpawnShrooms() {
            var shroomCells = new Cell[_numShrooms];
            for (int num = 0; num < _numShrooms; num++) {
                var cell = GetRandomCell(CellState.Grass);
                cell.State = CellState.Shroom;
                cell.Ctr = num;
                shroomCells[num] = cell;
            }
            return shroomCells;
        }

        private Cell GetRandomCell(CellState? state = null) {
            Cell cell;
            do { cell = _cells[_rnd.Next(0, _fieldSize)][_rnd.Next(0, _fieldSize)]; }
            while (state != null && cell.State != state);
            return cell;
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
