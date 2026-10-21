using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace old_bruteforcer_rewrite_5
{
    enum StableRange
    {
        None,
        Upper,
        Both
    }

    internal class PlayerRange
    {
        const int MAX_LENGTH = 1000; // TODO: global setting
        static float Floor = 408, Ceiling = 0, SolutionYUpper = 0, SolutionYLower = 0;
        static Stack<PlayerRange> PlayerRanges = new();
        float StartYUpper, StartYLower, YUpper, YLower;
        double VSpeed;
        public int Frame;
        bool HasSJump, HasDJump, Released;
        Event CurrentEvent; // TODO: i dont think this is something that needs copying
        List<Input> Inputs;

        public PlayerRange(float yUpper, float yLower, double vspeed, bool hasSJump, bool hasDJump)
        {
            if (yUpper > yLower)
            {
                (yUpper, yLower) = (yLower, yUpper);
            }

            CurrentEvent = Event.None;
            StartYUpper = YUpper = yUpper;
            StartYLower = YLower = yLower;
            VSpeed = vspeed;
            Frame = 0;
            HasDJump = hasDJump;
            HasSJump = hasSJump;
            Released = VSpeed >= 0; // avoid automatic release on first frame
            Inputs = [];
        }

        public PlayerRange(float startYUpper, float startYLower, float yUpper, float yLower, double vspeed, int frame, bool hasSJump, bool hasDJump, bool released, Event currentState, List<Input> inputs)
        {
            if (startYUpper > startYLower)
            {
                (startYUpper, startYLower) = (startYLower, startYUpper);
            }
            if (yUpper > yLower)
            {
                (yUpper, yLower) = (yLower, yUpper);
            }

            CurrentEvent = currentState;
            StartYUpper = startYUpper;
            StartYLower = startYLower;
            YUpper = yUpper;
            YLower = yLower;
            VSpeed = vspeed;
            Frame = frame;
            HasDJump = hasDJump;
            HasSJump = hasSJump;
            Released = released;
            Inputs = new List<Input>(inputs);
        }

        // create a copy of this PlayerRange
        public PlayerRange Copy()
        {
            return new PlayerRange(StartYUpper, StartYLower, YUpper, YLower, VSpeed, Frame, HasSJump, HasDJump, Released, CurrentEvent, Inputs);
        }

        // create a copy of this PlayerRange with a different range
        public PlayerRange Copy(float newStartYUpper, float newStartYLower, float newYUpper, float newYLower, double newVSpeed)
        {
            return new PlayerRange(newStartYUpper, newStartYLower, newYUpper, newYLower, newVSpeed, Frame, HasSJump, HasDJump, Released, CurrentEvent, Inputs);
        }

        // assumes the new range is within the current range
        public PlayerRange CopyRange(float newYUpper, float newYLower)
        {
            float newStartYUpper = StartYUpper + (newYUpper - YUpper);
            float newStartYLower = StartYLower - (YLower - newYLower);
            return new PlayerRange(newStartYUpper, newStartYLower, newYUpper, newYLower, VSpeed, Frame, HasSJump, HasDJump, Released, CurrentEvent, Inputs);
        }

        // assumes the lowest part of the range is stable
        public PlayerRange GetStableRange()
        {
            float highestCollision = Floor - 0.5f;
            if (MathF.Round(highestCollision) < Floor)
            {
                highestCollision = float.BitIncrement(highestCollision);
            }

            return CopyRange(MathF.Max(YUpper, highestCollision - (float)PhysicsParams.GRAVITY), YLower);
        }

        // assumes lowest part of the range can rejump
        public PlayerRange GetRejumpRange()
        {
            float highestCollision = Floor - 0.5f;
            if (MathF.Round(highestCollision) < Floor)
            {
                highestCollision = float.BitIncrement(highestCollision);
            }

            return CopyRange(MathF.Max(YUpper, highestCollision - 1), YLower);
        }

        // assumes this range intersects solution range
        public PlayerRange GetIntersectingSolutionRange()
        {
            return CopyRange(MathF.Max(YUpper, SolutionYUpper), MathF.Min(YLower, SolutionYLower));
        }

        public Event GetCurrentState()
        {
            return CurrentEvent;
        }

        public static void ClearPlayerRanges()
        {
            PlayerRanges.Clear();
        }

        public static void SetFloorY(string floorY)
        {
            Floor = MathF.Round(float.Parse(floorY, CultureInfo.InvariantCulture));
        }

        public static void SetCeilingY(string ceilingY)
        {
            Ceiling = MathF.Round(float.Parse(ceilingY, CultureInfo.InvariantCulture));
        }

        public static void SetSolutionYUpper(float y)
        {
            SolutionYUpper = y;
        }

        public static void SetSolutionYLower(float y)
        {
            SolutionYLower = y;
        }

        public bool CanPress()
        {
            return HasSJump || HasDJump;
        }

        public bool CanRelease()
        {
            return VSpeed < 0;
        }

        public bool CanRejump()
        {
            return MathF.Round(YLower + 1) >= Floor;
        }

        public bool ContainsExactYSolution()
        {
            return SolutionYUpper >= YUpper && SolutionYUpper <= YLower;
        }

        public bool IntersectsYSolutionRange()
        {
            return !(SolutionYLower < YUpper || SolutionYUpper > YLower);
        }

        // to make the two ranges disjoint after splitting, one of the ranges needs to be adjusted
        enum BitShift
        {
            UpperRange,
            LowerRange
        }

        PlayerRange SplitOffLowerAt(float split, BitShift range)
        {
            // split off lower range
            PlayerRange lower = Copy(StartYLower - YLower + split, StartYLower, split, YLower, VSpeed);
            
            // update current range to exclude split off range (both contain the split point)
            YLower = split;
            StartYLower = StartYUpper - YUpper + split;

            // make disjoint at split
            if (range == BitShift.UpperRange)
            {
                YLower = float.BitDecrement(YLower);
                StartYLower = float.BitDecrement(StartYLower);
            }
            else
            {
                lower.YUpper = float.BitIncrement(lower.YUpper);
                lower.StartYUpper = float.BitIncrement(lower.StartYUpper);
            }

            return lower;
        }

        PlayerRange SplitOffUpperAt(float split, BitShift range)
        {
            // split off upper range
            PlayerRange upper = Copy(StartYUpper, StartYUpper - YUpper + split, YUpper, split, VSpeed);

            // update current range to exclude split off range (both contain the split point)
            YUpper = split;
            StartYUpper = StartYLower - YLower + split;

            // make disjoint at split
            if (range == BitShift.UpperRange)
            {
                upper.YLower = float.BitDecrement(upper.YLower);
                upper.StartYLower = float.BitDecrement(upper.StartYLower);
            }
            else
            {
                YUpper = float.BitIncrement(YUpper);
                StartYUpper = float.BitIncrement(StartYUpper);
            }

            return upper;
        }

        public bool IsStable()
        {
            return VSpeed == 0 && MathF.Round(YLower + (float)PhysicsParams.GRAVITY) >= Floor;
        }

        public PlayerRange SplitOffStable()
        {
            // unstable, unnecessary if only called when IsStable is true, thus
            // TODO: make sure IsStable is used to determine whether to call this function
            if (VSpeed != 0 || MathF.Round(YLower + (float)PhysicsParams.GRAVITY) < Floor)
            {
                return null;
            }
            
            // full range stable
            if (MathF.Round(YUpper + (float)PhysicsParams.GRAVITY) >= Floor)
            {
                return this;
            }

            // partially stable
            float highestCollision = Floor - 0.5f;
            if (MathF.Round(highestCollision) < Floor)
            {
                highestCollision = float.BitIncrement(highestCollision);
            }

            PlayerRange lower = SplitOffLowerAt(highestCollision - (float)PhysicsParams.GRAVITY, BitShift.UpperRange);
            return lower;
        }

        // TODO: make sure to not create new lists all the time because performance and memory usage etc
        public List<PlayerRange> Step(Input input) // TODO: killers, return, debug log?
        {
            if (Frame >= MAX_LENGTH)
            {
                CurrentEvent = Event.Dead;
                return [];
            }

            CurrentEvent = Event.None;

            bool press = (input & Input.Press) == Input.Press;
            // shift press
            if (press)
            {
                if (!Released)
                {
                    CurrentEvent |= Event.WindowTrick;
                }
                if (HasSJump)
                {
                    VSpeed = PhysicsParams.SJUMP;
                }
                else if (HasDJump)
                {
                    VSpeed = PhysicsParams.DJUMP;
                    HasDJump = false;
                }
                else
                {
                    throw new Exception("Cannot press on this frame"); // TODO: remove
                }
                Released = false;
            }

            // TODO: what if use last results?
            if (Frame == 0)
            {
                // HasSJump is still true even if used so set to false here
                if (HasSJump)
                {
                    HasSJump = false;
                    // TODO: i think this behaviour should be optional, walkoff djump is not wrong in general
                    HasDJump &= press; // if not pressed, also remove djump
                }
                else
                {
                    // if djump not used on first frame and sjump not availble, remove
                    HasDJump = false;
                }
            }

            // shift release or automatic release
            if ((input & Input.Release) == Input.Release) // TODO: this should not release automatically for walkoff
            {
                if (VSpeed >= 0)
                {
                    throw new Exception("Cannot release on this frame"); // TODO: remove
                }

                if (Released)
                {
                    CurrentEvent |= Event.Cactus;
                }

                VSpeed *= PhysicsParams.RELEASE_MULTIPLIER;
                Released = true;
            }
            else if (!Released && VSpeed >= 0)
            {
                // automatic release for strat string
                Released = true;
                input |= Input.Release;
            }

            // add inputs to history before collision but after input processing
            Inputs.Add(input);
            Frame++;

            // max vspeed
            if (VSpeed > PhysicsParams.MAX_VSPEED)
            {
                VSpeed = PhysicsParams.MAX_VSPEED;
            }

            // gravity
            VSpeed += PhysicsParams.GRAVITY;

            List<PlayerRange> ranges;

            // one-way floor/ceiling
            if (VSpeed < 0)
            {
                // ceiling collision
                ranges = CeilingCollision();
            }
            else
            {
                // floor collision
                ranges = FloorCollision();
            }

            return ranges;
        }

        List<PlayerRange> FloorCollision()
        {
            float highestCollision = Floor - 0.5f;
            if (MathF.Round(highestCollision) < Floor)
            {
                highestCollision = float.BitIncrement(highestCollision);
            }

            List<PlayerRange> ranges = [];

            // get stuck range
            if (YLower >= highestCollision)
            {
                if (YUpper < highestCollision)
                {
                    // partially stuck
                    PlayerRange lower = SplitOffLowerAt(highestCollision, BitShift.UpperRange);
                    lower.VSpeed = 0;
                    lower.CurrentEvent |= Event.Landed;
                    ranges.Add(lower);
                }
                else
                {
                    // full range stuck
                    VSpeed = 0;
                    CurrentEvent |= Event.Landed;
                    return ranges;
                }
            }

            // separate between collision and no collision
            float highestCollisionAfterVspeed = highestCollision - (float)VSpeed;
            if (YLower >= highestCollisionAfterVspeed)
            {
                if (YUpper < highestCollisionAfterVspeed)
                {
                    // partial collision after vspeed
                    PlayerRange upper = SplitOffUpperAt(highestCollisionAfterVspeed, BitShift.UpperRange);
                    upper.YUpper += (float)upper.VSpeed;
                    upper.YLower += (float)upper.VSpeed;
                    ranges.Add(upper);
                }
                else
                {
                    // full range colliding after vspeed
                    // nothing more to do in this case
                }
            }
            else
            {
                // full range free
                YUpper += (float)VSpeed;
                YLower += (float)VSpeed;
                return ranges;
            }

            // the remaining range will land => vspeed = 0
            VSpeed = 0;
            CurrentEvent |= Event.Landed;

            // move_contact_solid
            float highestFloorReject = highestCollision - 1;
            while (YUpper < highestFloorReject)
            {
                if (YLower >= highestFloorReject)
                {
                    PlayerRange lower = SplitOffLowerAt(highestFloorReject, BitShift.UpperRange);
                    ranges.Add(lower);
                }
                YUpper += 1;
                YLower += 1;
            }

            // TODO: what if the remaining range is negative? can that even happen? make sure this is a valid range or invalidated in some way ig

            return ranges;
        }

        List<PlayerRange> CeilingCollision()
        {
            float lowestCollision = Ceiling + 0.5f;
            if (MathF.Round(lowestCollision) > Ceiling)
            {
                lowestCollision = float.BitDecrement(lowestCollision);
            }

            List<PlayerRange> ranges = [];

            // get stuck range
            // TODO: im not actually sure how i wanna handle ranges stuck in ceiling rn huh
            if (YUpper <= lowestCollision)
            {
                if (YLower > lowestCollision)
                {
                    // partially stuck
                    PlayerRange upper = SplitOffUpperAt(lowestCollision, BitShift.LowerRange);
                    upper.VSpeed = 0;
                    ranges.Add(upper);
                }
                else
                {
                    // full range stuck
                    VSpeed = 0;
                    return ranges;
                }
            }

            // separate between collision and no collision
            float lowestCollisionAfterVspeed = lowestCollision - (float)VSpeed;
            if (YUpper <= lowestCollisionAfterVspeed)
            {
                if (YLower > lowestCollisionAfterVspeed)
                {
                    // partial collision after vspeed
                    PlayerRange lower = SplitOffLowerAt(lowestCollisionAfterVspeed, BitShift.LowerRange);
                    lower.YUpper += (float)lower.VSpeed;
                    lower.YLower += (float)lower.VSpeed;
                    ranges.Add(lower);
                }
                else
                {
                    // full range colliding after vspeed
                    // nothing more to do in this case
                }
            }
            else
            {
                // full range free
                YUpper += (float)VSpeed;
                YLower += (float)VSpeed;
                return ranges;
            }

            // the remaining range will land => vspeed = 0
            VSpeed = 0;

            // move_contact_solid
            float lowestCeilingReject = lowestCollision + 1;
            while (YLower > lowestCeilingReject)
            {
                if (YUpper <= lowestCeilingReject)
                {
                    PlayerRange upper = SplitOffUpperAt(lowestCeilingReject, BitShift.LowerRange);
                    ranges.Add(upper);
                }
                YUpper -= 1;
                YLower -= 1;
            }

            // TODO: what if the remaining range is negative? can that even happen? make sure this is a valid range or invalidated in some way ig

            return ranges;
        }

        public bool DoStrat(string strat)
        {
            strat = strat.Trim().ToLower();
            bool pendingRelease = false;

            string[] presses = strat.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (string press in presses)
            {
                string[] releases = press.Split('+');
                foreach (string release in releases)
                {
                    if (release[^1] == 'f')
                    {
                        if (pendingRelease)
                        {
                            Step(Input.Release);
                            pendingRelease = false;
                        }

                        int frame = int.Parse(release[..^1]);
                        Input input = Input.Press;

                        for (int i = 0; i < frame; i++)
                        {
                            Step(input);
                            input = Input.None;
                        }

                        if (CanRelease() || (input & Input.Press) == Input.Press)
                        {
                            if (frame > 0)
                            {
                                pendingRelease = true;
                            }
                            else
                            {
                                input |= Input.Release;
                                Step(input);
                            }
                        }
                        else
                        {
                            Step(input);
                        }
                    }
                    else if (release[^1] == 'p')
                    {
                        int frame = int.Parse(release[..^1]);

                        if (pendingRelease && frame != 0)
                        {
                            Step(Input.Release);
                        }

                        for (int i = 0; i < frame - 1; i++)
                        {
                            Step(Input.None);
                        }

                        pendingRelease = false;
                    }
                    else
                    {
                        if (pendingRelease)
                        {
                            Step(Input.Release);
                            pendingRelease = false;
                        }

                        int frame = int.Parse(release);
                        for (int i = 0; i < frame - 1; i++)
                        {
                            Step(Input.None);
                        }
                        Step(Input.Release);
                    }
                }
            }

            if (pendingRelease)
            {
                Step(Input.Release);
                pendingRelease = false;
            }

            return true;
        }

        public string GetStrat(bool oneframeConvention)
        {
            if (Inputs.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new();
            int frame = 0;
            bool released = true; // dependant on starting vspeed to distinguish fixed vspeed jump and walkoff?

            /*
            1f 3p 4f 12p
            3p 3f 3p
            0f+1+1+1 10p
            3f 0p 5f 14p
            */

            foreach (Input input in Inputs)
            {
                if ((input & Input.Press) == Input.Press)
                {
                    if (frame > 0)
                    {
                        if (released)
                        {
                            sb.Append($" {frame}p");
                        }
                        else
                        {
                            sb.Append($" {(oneframeConvention ? frame + 1 : frame)}f 0p");
                        }
                    }

                    released = false;
                    frame = 0;
                }
                if ((input & Input.Release) == Input.Release)
                {
                    if (released)
                    {
                        sb.Append($"+{frame}");
                    }
                    else
                    {
                        sb.Append($" {(oneframeConvention ? frame + 1 : frame)}f");
                    }

                    released = true;
                    frame = 0;
                }

                frame++;
            }

            if (released)
            {
                sb.Append($" {frame}p");
            }
            else
            {
                sb.Append($" {(oneframeConvention ? frame + 1 : frame)}f");
            }

            return sb.ToString().Trim();
        }

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}] => [{2}, {3}] ({4})", StartYUpper, StartYLower, YUpper, YLower, VSpeed);

        // based on https://github.com/namelessiw/Jump-Bruteforcer/blob/fe878d1c5a625660ca5baa6abc0e47100ad34116/Jump_Bruteforcer/SearchOutput.cs#L59 (12.06.2024)
        public string GetMacro()
        {
            StringBuilder sb = new();

            foreach (Input input in Inputs)
            {
                bool InputChanged = false;

                if ((input & Input.Press) == Input.Press)
                {
                    sb.Append((InputChanged ? "," : "") + "J(PR)");

                    InputChanged = true;
                }
                if ((input & Input.Release) == Input.Release)
                {
                    sb.Append((InputChanged ? "," : "") + "K(PR)");
                }

                sb.Append('>');
            }

            return sb.ToString();
        }

        public static int Compare(PlayerRange a, PlayerRange b)
        {
            return a.Frame - b.Frame;
        }
    }
}
