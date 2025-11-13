using Forager.Core.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forager.Core {
    public record Route(Cell Source, Cell Target, Cell[] Path) { public int Cost = Path.Length - 1; }

    public record Router(Cell[] Cells, GameState State) {
        public Route[][] GetMatrix() => [.. Cells.Select(GetArray)];

        private Route[] GetArray(Cell source) => GetArray(source, GetPrev(source));

        private Route[] GetArray(Cell source, Dictionary<Cell, Cell> prev) =>
            [.. Cells.Select(t => new Route(source, t, GetPath(source, t, prev)))];

        private Dictionary<Cell, Cell> GetPrev(Cell source) {
            var prev = new Dictionary<Cell, Cell>() { [source] = source };
            var queue = new Queue<Cell>();
            queue.Enqueue(source);

            while (queue.Count > 0) {
                var curCell = queue.Dequeue();

                foreach (var nbCell in State.GetNeighbours(curCell)) {
                    if (nbCell.State == CellState.Stone)
                        continue;

                    if (prev.ContainsKey(nbCell))
                        continue;

                    prev[nbCell] = curCell;
                    queue.Enqueue(nbCell);
                }
            }

            return prev;
        }

        private static Cell[] GetPath(Cell source, Cell target, Dictionary<Cell, Cell> prev) =>
            Unpack(source, target, prev).Reverse().ToArray();

        private static IEnumerable<Cell> Unpack(Cell source, Cell target, Dictionary<Cell, Cell> prev) {
            var cell = target;
            while (cell != source) {
                yield return cell;
                cell = prev[cell];
            }
            yield return source;
        }
    }
}
