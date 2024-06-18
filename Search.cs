using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace old_bruteforcer_rewrite_5
{
    internal static class Search
    {
        public static List<PlayerRange> SearchRange()
        {
            Stack<PlayerRange> activeRanges = new([new(406.5, 406.5, 0, true, true)]);

            List<PlayerRange> results = [], ranges;

            while (activeRanges.Count > 0)
            {
                PlayerRange p = activeRanges.Peek(), temp;

                if (p.IsStable())
                {
                    PlayerRange stable = p.SplitOffStable();

                    if (p == stable)
                    {
                        activeRanges.Pop();
                        results.Add(p);
                        continue;
                    }
                    else
                    {
                        results.Add(stable);
                    }
                }

                if (p.CanPress())
                {
                    temp = p.Copy();
                    ranges = temp.Step(Input.Press);
                    foreach (PlayerRange range in ranges)
                    {
                        activeRanges.Push(range);
                    }
                    activeRanges.Push(temp);

                    temp = p.Copy();
                    ranges = temp.Step(Input.Press | Input.Release);
                    foreach (PlayerRange range in ranges)
                    {
                        activeRanges.Push(range);
                    }
                    activeRanges.Push(temp);
                }

                if (p.CanRelease())
                {
                    temp = p.Copy();
                    ranges = temp.Step(Input.Release);
                    foreach (PlayerRange range in ranges)
                    {
                        activeRanges.Push(range);
                    }
                    activeRanges.Push(temp);
                }

                ranges = p.Step(Input.None);
                foreach (PlayerRange range in ranges)
                {
                    activeRanges.Push(range);
                }
            }

            return results;
        }

        public static List<Player> SearchExact()
        {
            Stack<Player> activePlayers = new([new(406.5, 0, true, true)]);

            List<Player> results = [];

            while (activePlayers.Count > 0)
            {
                Player p = activePlayers.Peek(), temp;

                if (p.IsStable())
                {
                    activePlayers.Pop();
                    results.Add(p);
                    continue;
                }

                if (p.CanPress())
                {
                    temp = p.Copy();
                    temp.Step(Input.Press);
                    activePlayers.Push(temp);

                    temp = p.Copy();
                    temp.Step(Input.Press | Input.Release);
                    activePlayers.Push(temp);
                }

                if (p.CanRelease())
                {
                    temp = p.Copy();
                    temp.Step(Input.Release);
                    activePlayers.Push(temp);
                }

                p.Step(Input.None);
            }

            return results;
        }
    }
}
