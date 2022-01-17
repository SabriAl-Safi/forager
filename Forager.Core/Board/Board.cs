using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forager.Core.Board {
    public class Board {
        private int _fieldSize = 10;
        private Cell[][] _cells;
        private Cell _lastClicked;
        private HashSet<Cell> _tourCells;
        private Cell[] _shroomCells;
        private int _tourDistance;
        private int _goalDistance;
        private int _numInTour;
        private Cell _start;

        public Board() {
            for (int i = 0; i < _fieldSize; i++) {
                for (int j = 0; j < _fieldSize; j++) {
                    _cells[i][j].State = CellState.Grass;
                }
            }

            var rnd = new Random();
            _shroomCells = new Cell[_fieldSize];
            var num = 0;
            while (num < _fieldSize) {
                var iM = rnd.Next(0, _fieldSize);
                var jM = rnd.Next(0, _fieldSize);
                var cell = _cells[iM][jM];
                if (cell.IsShroom)
                    continue;

                cell.State = CellState.Shroom;
                _shroomCells[num] = cell;
                num++;
            }

            _lastClicked = null;
            _tourCells = new HashSet<Cell>();
            var matrix = new int[_shroomCells.Length][];
            for (int i = 0; i < _shroomCells.Length; i++) {
                var source = _shroomCells[i];
                matrix[i] = new int[_shroomCells.Length];
                for (int j = 0; j < _shroomCells.Length; j++) {
                    matrix[i][j] = source.DistanceTo(_shroomCells[j]);
                }
            }

            var tsp = new TSP(matrix);
            var tour = tsp.Solve();
            _goalDistance = tour.Cost;
            _tourDistance = 0;
            _numInTour = 0;
        }

        public void Reset() {
        }

        public void Select() {
        }
    }
}
