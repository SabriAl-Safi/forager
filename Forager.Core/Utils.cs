using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forager.Core {
    public static class Utils {
        public static IEnumerable<T> Except<T>(this IEnumerable<T> values, T excluded) => values.Except(new List<T> { excluded });

        public readonly static Color[] Colours = new Color[] {
            Color.Blue, Color.Red, Color.Brown, Color.Purple, Color.Yellow, Color.Lavender, Color.Orange, Color.Violet, Color.Black, Color.Turquoise
        };
    }
}
