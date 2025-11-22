using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forager.Core.Board {
    public record Cell(int Row, int Col) {
        public CellState State { get; set; } = CellState.Grass;
        public int Ctr { get; set; } = 0;
        public bool IsTrodden { get; set; } = false;
        public int NumSteps => (State == CellState.Woods ? 2 : 1);
    }
}
