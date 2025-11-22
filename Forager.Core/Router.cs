using Forager.Core.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forager.Core {
    public record Route(Cell Source, Cell Target, Cell[] Path) {
        public int Cost = Path.Take(Path.Length - 1).Sum(c => c.NumSteps);
    }

    public record Router(Cell[] Cells, GameState State) {
        public Route[][] GetMatrix() => [.. Cells.Select(GetArray)];

        private Route[] GetArray(Cell source) => GetArray(source, GetPrev(source));

        private Route[] GetArray(Cell source, Dictionary<Cell, Cell> prev) =>
            [.. Cells.Select(t => new Route(source, t, GetPath(source, t, prev)))];

        private Dictionary<Cell, Cell> GetPrev(Cell source) {
            var prev = new Dictionary<Cell, Cell>() { [source] = source };
            var bestSteps = new Dictionary<Cell, int>() { [source] = 0 };
            var queue = new Queue<Cell>();
            queue.Enqueue(source);

            while (queue.Count > 0) {
                var cur = queue.Dequeue();
                var bestCur = bestSteps[cur];

                foreach (var nxt in State.GetNeighbours(cur)) {
                    if (nxt.State == CellState.Stone)
                        continue;

                    if (bestSteps.TryGetValue(nxt, out var bestNxt) && bestNxt < bestCur + cur.NumSteps)
                        continue;

                    prev[nxt] = cur;
                    bestSteps[nxt] = bestCur + cur.NumSteps;
                    queue.Enqueue(nxt);
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
