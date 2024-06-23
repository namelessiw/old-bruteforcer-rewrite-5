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
        public static List<PlayerRange> SearchRange(string floor, string ceiling, double yUpper, double yLower, double vspeed, bool sjump, bool djump)
        {
            PlayerRange.SetFloorY(floor);
            PlayerRange.SetCeilingY(ceiling);
            Stack<PlayerRange> activeRanges = new([new(yUpper, yLower, vspeed, sjump, djump)]);

            List<PlayerRange> results = [], ranges;
            PlayerRange p, temp;

            // first frame
            p = activeRanges.Peek();
            SimulateStep(p);

            // simulate until stable
            while (activeRanges.Count > 0)
            {
                p = activeRanges.Peek();

                // result condition
                if (p.CanRejump())
                {
                    results.Add(p.GetRejumpRange());
                }

                // end condition
                if (p.IsStable())
                {
                    PlayerRange stable = p.SplitOffStable();

                    if (p == stable)
                    {
                        activeRanges.Pop();
                        continue;
                    }
                }

                SimulateStep(p);
            }

            void SimulateStep(PlayerRange p)
            {
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

        // TODO: take string arguments and forward to first player instance for parsing, then do error handling
        public static List<Player> SearchExact(string floor, string ceiling, double y, double vspeed, bool sjump, bool djump)
        {
            Player.SetFloorY(floor);
            Player.SetCeilingY(ceiling);
            Stack<Player> activePlayers = new([new(y, vspeed, sjump, djump)]);

            List<Player> results = [];
            Player p, temp;
            State state;

            // first frame
            p = activePlayers.Peek();
            SimulateStep(p);

            // simulate until stable
            while (activePlayers.Count > 0)
            {
                p = activePlayers.Peek();

                // result condition
                if (p.CanRejump())
                {
                    results.Add(p.Copy());
                }

                // end condition
                if (p.IsStable())
                {
                    activePlayers.Pop();
                    continue;
                }

                SimulateStep(p);
            }

            // simulate step with all possible inputs
            void SimulateStep(Player p)
            {
                if (p.CanPress())
                {
                    Step(p, Input.Press);
                    Step(p, Input.Press | Input.Release);
                }

                if (p.CanRelease())
                {
                    Step(p, Input.Release);
                }

                // avoid copying player here
                Step(p, Input.None, false);
            }

            // simulate step with specific input
            void Step(Player p, Input input, bool copy = true)
            {
                temp = copy ? p.Copy() : p;
                state = temp.Step(input);

                // dont consider dead player for results
                if ((state & State.Dead) == State.Dead)
                {
                    if (!copy)
                    {
                        activePlayers.Pop();
                    }
                    return;
                }

                if (copy)
                {
                    activePlayers.Push(temp);
                }
            }

            return results;
        }
    }
}
