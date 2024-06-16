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
        static double Floor = 408, Ceiling = 0;
        static Stack<PlayerRange> PlayerRanges = new();
        double StartYUpper, StartYLower, YUpper, YLower, VSpeed;
        int Frame;
        bool HasDJump, Released;
        List<Input> Inputs;

        public PlayerRange(double yUpper, double yLower, double vspeed, bool hasDJump)
        {
            if (yUpper > yLower)
            {
                (yUpper, yLower) = (yLower, yUpper);
            }

            StartYUpper = YUpper = yUpper;
            StartYLower = YLower = yLower;
            VSpeed = vspeed;
            Frame = 0;
            HasDJump = hasDJump;
            Released = false;
            Inputs = [];
        }

        public PlayerRange(double startYUpper, double startYLower, double yUpper, double yLower, double vspeed, int frame, bool hasDJump, bool released, List<Input> inputs)
        {
            if (startYUpper > startYLower)
            {
                (startYUpper, startYLower) = (startYLower, startYUpper);
            }
            if (yUpper > yLower)
            {
                (yUpper, yLower) = (yLower, yUpper);
            }

            StartYUpper = startYUpper;
            StartYLower = startYLower;
            YUpper = yUpper;
            YLower = yLower;
            VSpeed = vspeed;
            Frame = frame;
            HasDJump = hasDJump;
            Released = released;
            Inputs = new List<Input>(inputs);
        }

        public PlayerRange Copy()
        {
            return new PlayerRange((double)this.StartYUpper, (double)this.StartYLower, (double)this.YUpper, (double)this.YLower, (double)this.VSpeed, Frame, HasDJump, Released, Inputs);
        }

        public PlayerRange Copy(double newStartYUpper, double newStartYLower, double newYUpper, double newYLower, double newVSpeed)
        {
            return new PlayerRange(newStartYUpper, newStartYLower, newYUpper, newYLower, newVSpeed, Frame, HasDJump, Released, Inputs);
        }

        public static void ClearPlayerRanges()
        {
            PlayerRanges.Clear();
        }

        public static void SetFloorY(string floorY)
        {
            Floor = Math.Round(double.Parse(floorY, CultureInfo.InvariantCulture));
        }

        public static void SetCeilingY(string ceilingY)
        {
            Ceiling = Math.Round(double.Parse(ceilingY, CultureInfo.InvariantCulture));
        }

        public bool CanPress()
        {
            return Frame == 0 || HasDJump; // TODO: state variable?
        }

        public bool CanRelease()
        {
            return VSpeed < 0;
        }

        // to make the two ranges disjoint after splitting, one of the ranges needs to be adjusted
        enum BitShift
        {
            UpperRange,
            LowerRange
        }

        PlayerRange SplitOffLowerAt(double split, BitShift range)
        {
            // split off lower range
            PlayerRange lower = Copy(StartYLower - YLower + split, StartYLower, split, YLower, VSpeed);


            // update current range to exclude split off range (both contain the split point)
            YLower = split;
            StartYLower = StartYUpper - YUpper + split;

            // make disjoint at split
            if (range == BitShift.UpperRange)
            {
                YLower = double.BitDecrement(YLower);
                StartYLower = double.BitDecrement(StartYLower);
            }
            else
            {
                lower.YUpper = double.BitIncrement(lower.YUpper);
                lower.StartYUpper = double.BitIncrement(lower.StartYUpper);
            }

            return lower;
        }

        PlayerRange SplitOffUpperAt(double split, BitShift range)
        {
            // split off upper range
            PlayerRange upper = Copy(StartYUpper, StartYUpper - YUpper + split, YUpper, split, VSpeed);

            // update current range to exclude split off range (both contain the split point)
            YUpper = split;
            StartYUpper = StartYLower - YLower + split;

            // make disjoint at split
            if (range == BitShift.UpperRange)
            {
                upper.YLower = double.BitDecrement(upper.YLower);
                upper.StartYLower = double.BitDecrement(upper.StartYLower);
            }
            else
            {
                YUpper = double.BitIncrement(YUpper);
                StartYUpper = double.BitIncrement(StartYUpper);
            }

            return upper;
        }

        // temporary solution
        // TODO: need some way to split the range
        // TODO: return sets of player ranges stable, unstable?
        /*public StableRange IsStable()
        {
            // TODO: depends on one-way type
            if (VSpeed == 0)
            {
                if (Math.Round(YUpper + PhysicsParams.GRAVITY) >= Floor)
                {
                    if (Math.Round(YLower + PhysicsParams.GRAVITY) >= Floor)
                    {
                        return StableRange.Both;
                    }
                    return StableRange.Upper;
                }
                return StableRange.None;
            }
            return StableRange.None;
        }*/

        public PlayerRange SplitOffStable()
        {
            // unstable
            if (VSpeed != 0 || Math.Round(YLower + PhysicsParams.GRAVITY) < Floor)
            {
                return null;
            }
            
            // full range stable
            if (Math.Round(YUpper + PhysicsParams.GRAVITY) >= Floor)
            {
                return this;
            }

            // partially stable
            double highestCollision = Floor - 0.5;
            if (Math.Round(highestCollision) < Floor)
            {
                highestCollision = double.BitIncrement(highestCollision);
            }

            PlayerRange lower = SplitOffLowerAt(highestCollision - PhysicsParams.GRAVITY, BitShift.UpperRange);
            return lower;
        }

        public bool Step(Input input) // TODO: floor, ceiling, killers, return, debug log?
        {
            if (Frame >= MAX_LENGTH)
            {
                return false;
            }

            // shift press
            if ((input & Input.Press) == Input.Press)
            {
                if (Frame == 0) // TODO: state variable?
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

            // shift release or automatic release
            if ((input & Input.Release) == Input.Release) // TODO: this should not release automatically for walkoff
            {
                if (VSpeed >= 0)
                {
                    throw new Exception("Cannot release on this frame"); // TODO: remove
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

            // collision
            double YUpperPrevious = YUpper;
            double YLowerPrevious = YLower;
            YUpper += VSpeed;
            YLower += VSpeed;

            /*
            // higher, lower
            // stuck, free
            ceil
            342
            [341.5, 343.5] => [341.5, 342.5], (342.5, 343.5]

            343
            [342.5, 344.5] => [342.5, 343.5), [343.5, 344.5]


            // free, stuck
            floor
            408
            [406.5, 408.5] => [406.5, 407.5), [407.5, 408.5]

            409
            [407.5, 409.5] => [407.5, 408.5], (408.5, 409.5] 
            */

            // one-way floor/ceiling
            if (VSpeed < 0)
            {
                // ceiling collision
                if (Math.Round(YUpper) <= Ceiling)
                {
                    // TODO: have to do this differently!!
                    // first split, then eject?
                    double valign = YUpperPrevious - Math.Truncate(YUpperPrevious);
                    YUpper = Ceiling + valign;
                    if (Math.Round(YUpper) == Ceiling)
                    {
                        YUpper++;
                    }

                    if (Math.Round(YLower) <= Ceiling)
                    {
                        valign = YLowerPrevious - Math.Truncate(YLowerPrevious);
                        YLower = Ceiling + valign;
                        if (Math.Round(YLower) == Ceiling)
                        {
                            YLower++;
                        }

                        VSpeed = 0;

                        // if YUpper < YLower after collision, separated but both vspeed 0
                        if (YUpper < YLower)
                        {
                            // separate

                        }
                    }
                    else
                    {
                        // separate



                        VSpeed = 0;
                    }
                }



                // ceiling collision
                /*if (Math.Round(Y) <= Ceiling)
                {
                    double valign = YPrevious - Math.Truncate(YPrevious);
                    Y = Ceiling + valign;
                    if (Math.Round(Y) == Ceiling)
                    {
                        Y++;
                    }
                    VSpeed = 0;
                }*/
            }
            /*else
            {
                // floor collision
                if (Math.Round(Y) >= Floor)
                {
                    double valign = YPrevious - Math.Truncate(YPrevious);
                    Y = Floor - 1 + valign;
                    if (Math.Round(Y) == Floor)
                    {
                        Y--;
                    }
                    VSpeed = 0;
                }
            }*/

            return true;
        }

        public static List<PlayerRange> FloorCollision(PlayerRange p)
        {
            double highestCollision = Floor - 0.5;
            if (Math.Round(highestCollision) < Floor)
            {
                highestCollision = double.BitIncrement(highestCollision);
            }

            List<PlayerRange> ranges = [];

            // get stuck range
            if (p.YLower >= highestCollision)
            {
                if (p.YUpper < highestCollision)
                {
                    // partially stuck
                    PlayerRange lower = p.SplitOffLowerAt(highestCollision, BitShift.UpperRange);
                    lower.VSpeed = 0;
                    ranges.Add(lower);
                }
                else
                {
                    // full range stuck
                    p.VSpeed = 0;
                    return [p];
                }
            }

            // separate between collision and no collision
            double highestCollisionAfterVspeed = highestCollision - p.VSpeed;
            if (p.YLower >= highestCollisionAfterVspeed)
            {
                if (p.YUpper < highestCollisionAfterVspeed)
                {
                    // partial collision after vspeed
                    PlayerRange upper = p.SplitOffUpperAt(highestCollisionAfterVspeed, BitShift.UpperRange);
                    upper.YUpper += upper.VSpeed;
                    upper.YLower += upper.VSpeed;
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
                p.YUpper += p.VSpeed;
                p.YLower += p.VSpeed;
                ranges.Add(p);
                return ranges;
            }

            // the remaining range will land => vspeed = 0
            p.VSpeed = 0;

            // move_contact_solid
            double highestFloorReject = highestCollision - 1;
            while (p.YUpper < highestFloorReject)
            {
                if (p.YLower >= highestFloorReject)
                {
                    PlayerRange lower = p.SplitOffLowerAt(highestFloorReject, BitShift.UpperRange);
                    ranges.Add(lower);
                }
                p.YUpper += 1;
                p.YLower += 1;
            }

            // add remaining range
            if (p.YUpper < highestCollision)
            {
                ranges.Add(p);
            }

            return ranges;
        }

        public static List<PlayerRange> CeilingCollision(PlayerRange p)
        {
            double lowestCollision = Ceiling + 0.5;
            if (Math.Round(lowestCollision) > Ceiling)
            {
                lowestCollision = double.BitDecrement(lowestCollision);
            }

            List<PlayerRange> ranges = [];

            // get stuck range
            // TODO: im not actually sure how i wanna handle ranges stuck in ceiling rn huh
            if (p.YUpper <= lowestCollision)
            {
                if (p.YLower > lowestCollision)
                {
                    // partially stuck
                    PlayerRange upper = p.SplitOffUpperAt(lowestCollision, BitShift.LowerRange);
                    upper.VSpeed = 0;
                    ranges.Add(upper);
                }
                else
                {
                    // full range stuck
                    p.VSpeed = 0;
                    return [p];
                }
            }

            // separate between collision and no collision
            double lowestCollisionAfterVspeed = lowestCollision - p.VSpeed;
            if (p.YUpper <= lowestCollisionAfterVspeed)
            {
                if (p.YLower > lowestCollisionAfterVspeed)
                {
                    // partial collision after vspeed
                    PlayerRange lower = p.SplitOffLowerAt(lowestCollisionAfterVspeed, BitShift.LowerRange);
                    lower.YUpper += lower.VSpeed;
                    lower.YLower += lower.VSpeed;
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
                p.YUpper += p.VSpeed;
                p.YLower += p.VSpeed;
                ranges.Add(p);
                return ranges;
            }

            // the remaining range will land => vspeed = 0
            p.VSpeed = 0;

            // move_contact_solid
            double lowestCeilingReject = lowestCollision + 1;
            while (p.YLower > lowestCeilingReject)
            {
                if (p.YUpper <= lowestCeilingReject)
                {
                    PlayerRange upper = p.SplitOffUpperAt(lowestCeilingReject, BitShift.LowerRange);
                    ranges.Add(upper);
                }
                p.YUpper -= 1;
                p.YLower -= 1;
            }

            // add remaining range
            if (p.YLower > lowestCollision)
            {
                ranges.Add(p);
            }

            return ranges;
        }

        // separate current PlayerRange at pixel boundary depending on Y parity of solid
        // positionally lower: this, upper: other
        /*PlayerRange Separate(bool Floor)
        {
            PlayerRange other = Copy();

            other.YLower = Math.Round(YUpper) + 0.5;
            YUpper = Math.Round(YLower) - 0.5;

            if (Floor)
            {

            }
            else
            {

            }

            return other;
        }*/

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
            bool released = false; // dependant on starting vspeed to distinguish fixed vspeed jump and walkoff?

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

        public override string ToString()
        {
            return $"YLower: {YLower}\nYUpper: {YUpper}\nVSpeed: {VSpeed}\nFrame: {Frame}\nHasDJump: {HasDJump}\nReleased: {Released}\n";
        }

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
    }
}
