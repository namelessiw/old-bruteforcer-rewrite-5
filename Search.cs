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

            // simulate until stable
            while (activeRanges.Count > 0)
            {
                PlayerRange p = activeRanges.Peek();
                bool canPress = p.CanPress(), canRelease = p.CanRelease();

                // avoid copying the range unnecessarily
                if (canPress || canRelease)
                {
                    PlayerRange copy = p.Copy();

                    // the non-copy one needs to be done first to avoid unnecessary stack manipulation
                    // non-input variation first since it requires the least amount of extra logic
                    Step(p, Input.None, false);

                    if (canPress)
                    {
                        if (canRelease)
                        {
                            Step(copy.Copy(), Input.Release);
                        }
                        Step(copy.Copy(), Input.Press | Input.Release);
                        Step(copy, Input.Press);
                    }
                    else
                    {
                        Step(copy, Input.Release);
                    }
                }
                else
                {
                    // no need to copy the range here
                    Step(p, Input.None, false);
                }
            }

            // simulate step with specific input
            void Step(PlayerRange p, Input input, bool isCopy = true)
            {
                ranges = p.Step(input);

                ConditionalPush(p);

                foreach (PlayerRange range in ranges)
                {
                    ConditionalPush(range);
                }

                void ConditionalPush(PlayerRange range)
                {
                    // result condition
                    if (range.GetCurrentState() == State.Landed)
                    {
                        results.Add(range.Copy());
                    }

                    // end condition
                    if (range.IsStable())
                    {
                        PlayerRange stable = range.SplitOffStable();

                        if (range == stable)
                        {
                            if (range == p)
                            {
                                activeRanges.Pop();
                            }
                            return;
                        }
                    }

                    if (range == p)
                    {
                        if (isCopy)
                        {
                            activeRanges.Push(range);
                        }
                    }
                    else
                    {
                        activeRanges.Push(range);
                    }
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

            // simulate step with all possible inputs until stable
            while (activePlayers.Count > 0)
            {
                Player p = activePlayers.Peek();
                bool canPress = p.CanPress(), canRelease = p.CanRelease();

                // avoid copying the player unnecessarily
                if (canPress || canRelease)
                {
                    Player copy = p.Copy();

                    // the non-copy one needs to be done first to avoid unnecessary stack manipulation
                    // non-input variation first since it requires the least amount of extra logic
                    Step(p, Input.None, false);

                    if (canPress)
                    {
                        if (canRelease)
                        {
                            Step(copy.Copy(), Input.Release);
                        }
                        Step(copy.Copy(), Input.Press | Input.Release);
                        Step(copy, Input.Press);
                    }
                    else
                    {
                        Step(copy, Input.Release);
                    }
                }
                else
                {
                    // no need to copy the player here
                    Step(p, Input.None, false);
                }
            }

            // simulate step with specific input
            void Step(Player p, Input input, bool isCopy = true)
            {
                State state = p.Step(input);

                // dont consider dead player for results
                if ((state & State.Dead) == State.Dead)
                {
                    if (!isCopy)
                    {
                        activePlayers.Pop();
                    }
                    return;
                }

                // result condition
                if ((state & State.Landed) == State.Landed)
                {
                    results.Add(p.Copy());
                }

                // end condition
                if (p.IsStable())
                {
                    if (!isCopy)
                    {
                        activePlayers.Pop();
                    }
                    return;
                }

                if (isCopy)
                {
                    activePlayers.Push(p);
                }
            }

            return results;
        }
    }
}
