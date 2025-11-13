using Forager.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forager.Core {
    public class Cell {
        public int Row { get; set; }
        public int Col { get; set; }
        public CellState State { get; set; }
        public bool IsOriginal { get; set; }
        public bool IsShroom => State == CellState.Shroom;
        public int Ctr { get; set; } = 0;
        public bool IsTrodden { get; set; } = false;
    }
}
