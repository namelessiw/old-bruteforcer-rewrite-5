using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace old_bruteforcer_rewrite_5
{
    [Flags]
    enum Input
    {
        None = 0,
        Press = 1,
        Release = 2,
    }

    internal class Player
    {
        const double GRAVITY = 0.4, SJUMP = -8.5, DJUMP = -7, MAX_VSPEED = 9, RELEASE_MULTIPLIER = 0.45; // TODO: MIN_VSPEED, move out physics params
        static int Floor = 408, Ceiling = 384; // TODO: these should be of type double aswell (for farlands) but ensure integers
        double Y, YPrevious, VSpeed; // TODO: YPrevious is used solely for collision, maybe not have it be a member variable
        int Frame;
        bool HasDJump, Released;
        List<Input> Inputs;

        public Player(double y, double vspeed, int frame, bool hasDJump, bool released, List<Input> inputs)
        {
            Y = y;
            VSpeed = vspeed;
            Frame = frame;
            HasDJump = hasDJump;
            Released = released;
            Inputs = new List<Input>(inputs);
        }

        public Player Copy()
        {
            return new Player(Y, VSpeed, Frame, HasDJump, Released, Inputs);
        }

        public bool CanJump()
        {
            return Frame == 0 || HasDJump; // TODO: state variable?
        }

        public bool IsStable()
        {
            return VSpeed == 0 && Math.Round(Y + GRAVITY) >= Floor; // TODO: depends on one-way type
        }

        public bool Step(Input input) // TODO: floor, ceiling, killers, return, debug log?
        {
            // shift press
            if ((input & Input.Press) == Input.Press)
            {
                if (Frame == 0) // TODO: state variable?
                {
                    VSpeed = SJUMP;
                }
                else if (HasDJump)
                {
                    VSpeed = DJUMP;
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

                VSpeed *= RELEASE_MULTIPLIER;
                Released = true;
            }
            else if (!Released && VSpeed >= 0)
            {
                // automatic release for strat string
                Released = true;
                input |= Input.Release;
            }

            // max vspeed
            if (VSpeed > MAX_VSPEED)
            {
                VSpeed = MAX_VSPEED;
            }

            // gravity
            VSpeed += GRAVITY;

            // collision
            YPrevious = Y;
            Y += VSpeed;

            // one-way floor/ceiling
            if (VSpeed < 0)
            {
                // ceiling collision
                if (Math.Round(Y) <= Ceiling)
                {
                    // move_contact_solid
                    Y = YPrevious;
                    do
                    {
                        YPrevious = Y;
                        Y--;
                    }
                    while (Math.Round(Y) > Ceiling);

                    Y = YPrevious;
                    VSpeed = 0;
                }
            }
            else
            {
                // floor collision
                if (Math.Round(Y) >= Floor)
                {
                    // move_contact_solid
                    Y = YPrevious;
                    do
                    {
                        YPrevious = Y;
                        Y++;
                    }
                    while (Math.Round(Y) < Floor);

                    Y = YPrevious;
                    VSpeed = 0;
                }
            }

            // finalize
            Inputs.Add(input);
            Frame++;
            return true;
        }

        public string GetStrat(bool oneframeConvention)
        {
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

            if (frame > 0)
            {
                if (released)
                {
                    sb.Append($" {frame}p");
                }
                else
                {
                    sb.Append($" {(oneframeConvention ? frame + 1 : frame)}f");
                }
            }

            return sb.ToString().Trim();
        }

        public override string ToString()
        {
            return $"Y: {Y}\nVSpeed: {VSpeed}\nFrame: {Frame}\nHasDJump: {HasDJump}\nReleased: {Released}\nInputs:\n{string.Join("\n", Inputs)}";
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
