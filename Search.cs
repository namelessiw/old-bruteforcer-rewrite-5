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

        delegate bool ResultConditionExact(Player p, State state);
        delegate void ResultConditionRange(PlayerRange p, State state);

        // TODO: take string arguments and forward to first player instance for parsing, then do error handling
        public static List<Player> SearchExact(bool sjump, bool djump, SolutionCondition solutionCondition)
        {
            Stack<Player> activePlayers;
            try
            {
                Player.SetFloorY(SearchParams.FloorY);
                Player.SetCeilingY(SearchParams.CeilingY);
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

            ResultConditionExact CheckResultCondition = solutionCondition switch
            {
                SolutionCondition.CanRejump => CheckCanRejump,
                SolutionCondition.Landed => CheckLanded,
                SolutionCondition.Stable => CheckStable,
                _ => throw new Exception($"unimplemented solution condition {solutionCondition}")
            };

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
                if (CheckResultCondition(p, state))
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

            static bool CheckLanded(Player p, State state)
            {
                return (state & State.Landed) == State.Landed;
            }

            static bool CheckStable(Player p, State state)
            {
                return p.IsStable();
            }

            static bool CheckCanRejump(Player p, State state)
            {
                return p.CanRejump();
            }

            return results;
        }

        public static List<PlayerRange> SearchRange(bool sjump, bool djump, SolutionCondition solutionCondition)
        {
            Stack<PlayerRange> activeRanges;
            try
            {
                PlayerRange.SetFloorY(SearchParams.FloorY);
                PlayerRange.SetCeilingY(SearchParams.CeilingY);
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

            ResultConditionRange CheckResultCondition = solutionCondition switch
            {
                SolutionCondition.CanRejump => CheckCanRejump,
                SolutionCondition.Landed => CheckLanded,
                SolutionCondition.Stable => CheckStable,
                _ => throw new Exception($"unimplemented solution condition {solutionCondition}")
            };

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
                    State state = range.GetCurrentState();
                    if ((state & State.Dead) == State.Dead)
                    {
                        if (range == p)
                        {
                            activeRanges.Pop();
                        }
                        return;
                    }

                    // result condition
                    CheckResultCondition(range, state);

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

            void CheckLanded(PlayerRange p, State state)
            {
                if ((state & State.Landed) == State.Landed)
                {
                    results.Add(p.Copy());
                }
            }

            void CheckStable(PlayerRange p, State state)
            {
                if (p.IsStable())
                {
                    results.Add(p.GetStableRange());
                }
            }

            void CheckCanRejump(PlayerRange p, State state)
            {
                if (p.CanRejump())
                {
                    results.Add(p.GetRejumpRange());
                }
            }

            return results;
        }
    }
}
