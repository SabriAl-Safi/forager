using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forager.Core {
    [TestFixture]
    public class TSPTests {

        [TestCase]
        public void SolveRecursiveTests() {
            var matrix = new int[][] { [2, 1, 2], [2, 2, 1], [1, 2, 2] };
            var tsp = new TSP(matrix);
            var expected = new Tour { Cost = 3, Nodes = [0, 1, 2, 0], Remaining = [] };
            var actual = tsp.Solve();

            Assert.That(expected.Cost, Is.EqualTo(actual.Cost));
            Assert.That(expected.Nodes, Is.EquivalentTo(actual.Nodes));
        }
    }
}
