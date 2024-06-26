using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace old_bruteforcer_rewrite_5
{
    internal static class Search
    {
        static float ParseFloat(string s) => float.Parse(s, CultureInfo.InvariantCulture);
        static double ParseDouble(string s) => double.Parse(s, CultureInfo.InvariantCulture);

        delegate bool ResultConditionExact(Player p, Event state);
        delegate void ResultConditionRange(PlayerRange p, Event state);

        // TODO: take string arguments and forward to first player instance for parsing, then do error handling
        public static List<Player> SearchExact(bool sjump, bool djump, bool allowCactus, bool allowWindowTrick)
        {
            Stack<Player> activePlayers;
            try
            {
                Player.SetFloorY(SearchParams.FloorY);
                Player.SetCeilingY(SearchParams.CeilingY);
                double solutionYUpper = ParseDouble(SearchParams.SolutionYUpper);
                double solutionYLower = ParseDouble(SearchParams.SolutionYLower);
                if (solutionYUpper > solutionYLower)
                {
                    (solutionYUpper, solutionYLower) = (solutionYLower, solutionYUpper);
                }
                Player.SetSolutionYUpper(solutionYUpper);
                Player.SetSolutionYLower(solutionYLower);
                double y = ParseDouble(SearchParams.PlayerYLower);
                double vspeed = ParseDouble(SearchParams.PlayerVSpeed);
                activePlayers = new([new(y, vspeed, sjump, djump)]);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return [];
            }

            List<Player> results = [];

            ResultConditionExact CheckResultCondition = SearchParams.SolutionCondition switch
            {
                SolutionCondition.CanRejump => CheckCanRejump,
                SolutionCondition.Landed => CheckLanded,
                SolutionCondition.Stable => CheckStable,
                SolutionCondition.ExactY => CheckExactSolution,
                SolutionCondition.YRange => CheckInSolutionRange,
                _ => throw new Exception($"unimplemented solution condition {SearchParams.SolutionCondition}")
            };

            Event filter = Event.Dead;
            if (!allowCactus)
            {
                filter |= Event.Cactus;
            }
            if (!allowWindowTrick)
            {
                filter |= Event.WindowTrick;
            }

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
                Event events = p.Step(input);

                // dont consider dead player for results
                if ((events & filter) != Event.None)
                {
                    if (!isCopy)
                    {
                        activePlayers.Pop();
                    }
                    return;
                }

                // result condition
                if (CheckResultCondition(p, events))
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

            static bool CheckLanded(Player p, Event state)
            {
                return (state & Event.Landed) == Event.Landed;
            }

            static bool CheckStable(Player p, Event state)
            {
                return p.IsStable();
            }

            static bool CheckCanRejump(Player p, Event state)
            {
                return p.CanRejump();
            }

            static bool CheckExactSolution(Player p, Event state)
            {
                return p.IsExactYSolution();
            }

            static bool CheckInSolutionRange(Player p, Event state)
            {
                return p.IsInYSolutionRange();
            }

            return results;
        }

        public static List<PlayerRange> SearchRange(bool sjump, bool djump, bool allowCactus, bool allowWindowTrick)
        {
            Stack<PlayerRange> activeRanges;
            try
            {
                PlayerRange.SetFloorY(SearchParams.FloorY);
                PlayerRange.SetCeilingY(SearchParams.CeilingY);
                double solutionYUpper = ParseDouble(SearchParams.SolutionYUpper);
                double solutionYLower = ParseDouble(SearchParams.SolutionYLower);
                if (solutionYUpper > solutionYLower)
                {
                    (solutionYUpper, solutionYLower) = (solutionYLower, solutionYUpper);
                }
                PlayerRange.SetSolutionYUpper(solutionYUpper);
                PlayerRange.SetSolutionYLower(solutionYLower);
                double yLower = ParseDouble(SearchParams.PlayerYLower);
                double yUpper = ParseDouble(SearchParams.PlayerYUpper);
                double vspeed = ParseDouble(SearchParams.PlayerVSpeed);
                activeRanges = new([new(yUpper, yLower, vspeed, sjump, djump)]);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return [];
            }

            List<PlayerRange> results = [], ranges;

            ResultConditionRange CheckResultCondition = SearchParams.SolutionCondition switch
            {
                SolutionCondition.CanRejump => CheckCanRejump,
                SolutionCondition.Landed => CheckLanded,
                SolutionCondition.Stable => CheckStable,
                SolutionCondition.ExactY => CheckContainsExactYSolution,
                SolutionCondition.YRange => CheckIntersectsYSolutionRange,
                _ => throw new Exception($"unimplemented solution condition {SearchParams.SolutionCondition}")
            };

            Event filter = Event.Dead;
            if (!allowCactus)
            {
                filter |= Event.Cactus;
            }
            if (!allowWindowTrick)
            {
                filter |= Event.WindowTrick;
            }

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
                    Event events = range.GetCurrentState();
                    if ((events & filter) != Event.None)
                    {
                        if (range == p && (events & Event.Dead) == Event.Dead)
                        {
                            activeRanges.Pop();
                        }
                        return;
                    }

                    // result condition
                    CheckResultCondition(range, events);

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

            void CheckLanded(PlayerRange p, Event state)
            {
                if ((state & Event.Landed) == Event.Landed)
                {
                    results.Add(p.Copy());
                }
            }

            void CheckStable(PlayerRange p, Event state)
            {
                if (p.IsStable())
                {
                    results.Add(p.GetStableRange());
                }
            }

            void CheckCanRejump(PlayerRange p, Event state)
            {
                if (p.CanRejump())
                {
                    results.Add(p.GetRejumpRange());
                }
            }

            void CheckContainsExactYSolution(PlayerRange p, Event state)
            {
                if (p.ContainsExactYSolution())
                {
                    results.Add(p.GetIntersectingSolutionRange());
                }
            }

            void CheckIntersectsYSolutionRange(PlayerRange p, Event state)
            {
                if (p.IntersectsYSolutionRange())
                {
                    results.Add(p.GetIntersectingSolutionRange());
                }
            }

            return results;
        }
    }
}
