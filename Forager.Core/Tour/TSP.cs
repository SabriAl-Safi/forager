using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forager.Core {
    public class TSP {
        private int[][] _matrix;

        public TSP (int[][] matrix) => _matrix = matrix;

        public Tour Solve() => SolveRecursive(Tour.Seed(0, _matrix.Length), int.MaxValue);

        public Tour SolveRecursive(Tour partialTour, int globalBestCost) {
            if (!partialTour.Remaining.Any()) {
                var newCost = partialTour.Cost + _matrix[partialTour.Nodes.Last()][0];
                if (newCost > globalBestCost)
                    return null;

                return new Tour {
                    Nodes = partialTour.Nodes.Concat(new List<int> { 0 }).ToList(),
                    Cost = newCost,
                    Remaining = partialTour.Remaining
                };
            }

            var localBestCost = globalBestCost;
            Tour localBestTour = null;
            foreach (var node in partialTour.Remaining) {
                var newCost = partialTour.Cost + _matrix[partialTour.Nodes.Last()][node];
                if (newCost > globalBestCost)
                    continue;

                var newTour = new Tour {
                    Nodes = partialTour.Nodes.Append(node).ToList(),
                    Cost = newCost,
                    Remaining = partialTour.Remaining.Except(node).ToHashSet()
                };

                var candidateTour = SolveRecursive(newTour, localBestCost);
                if (candidateTour != null && candidateTour.Cost < localBestCost) {
                    localBestCost = candidateTour.Cost;
                    localBestTour = candidateTour;
                }
            }

            return localBestTour;
        }
    }
}
