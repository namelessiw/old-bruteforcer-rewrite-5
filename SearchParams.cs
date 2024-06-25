using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace old_bruteforcer_rewrite_5
{
    enum SolutionCondition
    {
        CanRejump,
        Landed,
        Stable,
        YPosition,
        ExactY,
        YRange,
    }

    static internal class SearchParams
    {
        public static SolutionCondition SolutionCondition = SolutionCondition.CanRejump;
        public static string FloorY = "", CeilingY = "", PlayerYUpper = "", PlayerYLower = "", PlayerVSpeed = "", SolutionYUpper = "", SolutionYLower = "";
    }
}
