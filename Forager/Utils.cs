using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forager {
    public static class Utils {
        public static IEnumerable<T> Except<T>(this IEnumerable<T> values, T excluded) => values.Except(new List<T> { excluded });
    }
}
