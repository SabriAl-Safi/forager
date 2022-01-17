using Forager.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forager.cs {
    [TestFixture]
    public class TSPTests {

        [TestCase]
        public void SolveRecursiveTests() {
            var matrix = new int[][] {
                new int[] { 2, 1, 2 },
                new int[] { 2, 2, 1 },
                new int[] { 1, 2, 2 }
            };
            var tsp = new TSP(matrix);
            var expected = new Tour { Cost = 3, Nodes = new List<int> { 0, 1, 2, 0 }, Remaining = new HashSet<int>() };
            var actual = tsp.Solve();

            Assert.That(expected.Cost, Is.EqualTo(actual.Cost));
            Assert.That(expected.Nodes, Is.EquivalentTo(actual.Nodes));
        }
    }
}
