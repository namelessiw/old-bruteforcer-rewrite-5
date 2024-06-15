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
        double YUpper, YLower, VSpeed; // TODO: YPrevious is used solely for collision, maybe not have it be a member variable
        int Frame;
        bool HasDJump, Released;
        List<Input> Inputs;

        public PlayerRange(double yUpper, double yLower, double vspeed, int frame, bool hasDJump, bool released, List<Input> inputs)
        {
            if (yUpper > yLower)
            {
                (yUpper, yLower) = (yLower, yUpper);
            }

            YUpper = yUpper;
            YLower = yLower;
            VSpeed = vspeed;
            Frame = frame;
            HasDJump = hasDJump;
            Released = released;
            Inputs = new List<Input>(inputs);
        }

        public static List<(double upper, double lower)> test(double yUpper, double yLower, double floor, double vSpeed)
        {
            double highestCollision = floor - 0.5;
            if (Math.Round(highestCollision) < floor)
            {
                highestCollision = double.BitIncrement(highestCollision);
            }

            List<(double upper, double lower)> ranges = [];

            // get stuck range
            if (yLower >= highestCollision)
            {
                if (yUpper < highestCollision)
                {
                    // partially stuck
                    ranges.Add((highestCollision, yLower));
                    yLower = double.BitDecrement(highestCollision);
                }
                else
                {
                    // full range stuck
                    return [(yUpper, yLower)];
                }
            }

            // separate between collision and no collision
            double highestCollisionAfterVspeed = highestCollision - vSpeed;
            if (yLower >= highestCollisionAfterVspeed)
            {
                if (yUpper < highestCollisionAfterVspeed)
                {
                    // partial collision
                    ranges.Add((yUpper, double.BitDecrement(highestCollisionAfterVspeed)));
                    yUpper = highestCollisionAfterVspeed;
                }
                else
                {
                    // full range colliding
                }
            }

            // split remaining range into collision range
            double split = highestCollision - 1;
            while (split >= yUpper)
            {
                if (split > yLower)
                {
                    split--;
                    continue;
                }

                ranges.Add((split, yLower));
                yLower = double.BitDecrement(split);
                split--;
            }

            if (yUpper <= yLower)
            {
                ranges.Add((yUpper,yLower));
            }

            return ranges;
        }

        public PlayerRange Copy()
        {
            return new PlayerRange(YUpper, YLower, VSpeed, Frame, HasDJump, Released, Inputs);
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

        // temporary solution
        // TODO: need some way to split the range
        // TODO: return sets of player ranges stable, unstable?
        public StableRange IsStable()
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

        // separate current PlayerRange at pixel boundary depending on Y parity of solid
        // positionally lower: this, upper: other
        PlayerRange Separate(bool Floor)
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
