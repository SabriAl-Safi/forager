using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forager {
    public class GameCell {
        public PictureBox PictureBox { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public CellState State { get; set; }
        public bool IsShroom => State == CellState.Shroom;

        public int DistanceTo(GameCell other) => Math.Abs(Row - other.Row) + Math.Abs(Col - other.Col);
    }
}
