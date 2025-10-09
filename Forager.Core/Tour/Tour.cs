using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forager.Core {
    public class Tour {
        public List<int> Nodes { get; set; }
        public int Cost { get; set; } = 0;
        public HashSet<int> Remaining { get; set; }

        public static Tour Seed(int start, int numNodes) => new() {
            Nodes = [start],
            Remaining = [..Enumerable.Range(0, numNodes).Except([start])]
        };
    }
}
