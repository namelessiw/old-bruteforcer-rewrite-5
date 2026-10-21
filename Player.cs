using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace old_bruteforcer_rewrite_5
{
    [Flags]
    enum Input
    {
        None = 0,
        Press = 1,
        Release = 2,
    }

    [Flags]
    enum Event
    {
        None = 0,
        Dead = 1,
        Landed = 2,
        Cactus = 4,
        WindowTrick = 8,
    }

    internal class Player
    {
        const int MAX_LENGTH = 1000; // TODO: global setting
        static double Floor = 408, Ceiling = 0, SolutionYUpper = 0, SolutionYLower = 0;
        double Y, VSpeed;
        public int Frame;
        bool HasSJump, HasDJump, Released;
        List<Input> Inputs;

        public Player(double y, double vspeed, bool hasSJump, bool hasDJump)
        {
            Y = y;
            VSpeed = vspeed;
            Frame = 0;
            HasSJump = hasSJump;
            HasDJump = hasDJump;
            Released = VSpeed >= 0; // avoid automatic release on first frame
            Inputs = new List<Input>();
        }

        public Player(double y, double vspeed, int frame, bool hasSJump, bool hasDJump, bool released, List<Input> inputs)
        {
            Y = y;
            VSpeed = vspeed;
            Frame = frame;
            HasSJump = hasSJump;
            HasDJump = hasDJump;
            Released = released;
            Inputs = new List<Input>(inputs);
        }

        public Player Copy()
        {
            return new Player(Y, VSpeed, Frame, HasSJump, HasDJump, Released, Inputs);
        }

        public static void SetFloorY(string floorY)
        {
            Floor = Math.Round(double.Parse(floorY, CultureInfo.InvariantCulture));
        }

        public static void SetCeilingY(string ceilingY)
        {
            Ceiling = Math.Round(double.Parse(ceilingY, CultureInfo.InvariantCulture));
        }

        public static void SetSolutionYUpper(double y)
        {
            SolutionYUpper = y;
        }

        public static void SetSolutionYLower(double y)
        {
            SolutionYLower = y;
        }

        public bool CanPress()
        {
            return HasSJump || HasDJump; // TODO: state variable?
        }

        public bool CanRelease()
        {
            return VSpeed < 0;
        }

        public bool IsStable()
        {
            return VSpeed == 0 && Math.Round(Y + PhysicsParams.GRAVITY) >= Floor; // TODO: depends on one-way type
        }

        public bool CanRejump()
        {
            return Math.Round(Y + 1) >= Floor; // TODO: floor type matters
        }

        public bool IsExactYSolution()
        {
            return Y == SolutionYUpper;
        }

        public bool IsInYSolutionRange()
        {
            return Y >= SolutionYUpper && Y <= SolutionYLower;
        }

        public Event Step(Input input) // TODO: killers, return, debug log?
        {
            if (Frame >= MAX_LENGTH)
            {
                return Event.Dead;
            }


            Event events = Event.None;
            bool press = (input & Input.Press) == Input.Press;
            // shift press
            if (press)
            {
                if (!Released)
                {
                    events |= Event.WindowTrick;
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
                    // TODO: optional
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
                    events |= Event.Cactus;
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

            // max vspeed
            if (VSpeed > PhysicsParams.MAX_VSPEED)
            {
                VSpeed = PhysicsParams.MAX_VSPEED;
            }

            // gravity
            VSpeed += PhysicsParams.GRAVITY;

            // collision
            double yPrevious = Y;
            Y += VSpeed;

            // one-way floor/ceiling
            if (VSpeed < 0)
            {
                // ceiling collision
                if (Math.Round(Y) <= Ceiling)
                {
                    if (Math.Round(yPrevious) <= Ceiling)
                    {
                        Y = yPrevious;
                        VSpeed = 0;
                    }
                    else
                    {
                        double valign = yPrevious - Math.Truncate(yPrevious);
                        Y = Ceiling + valign;
                        if (Math.Round(Y) == Ceiling)
                        {
                            Y++;
                        }
                        VSpeed = 0;
                    }
                }
            }
            else
            {
                // floor collision
                if (Math.Round(Y) >= Floor)
                {
                    if (Math.Round(yPrevious) >= Floor)
                    {
                        Y = yPrevious;
                        VSpeed = 0;
                    }
                    else
                    {
                        double valign = yPrevious - Math.Truncate(yPrevious);
                        Y = Floor - 1 + valign;
                        if (Math.Round(Y) == Floor)
                        {
                            Y--;
                        }
                        VSpeed = 0;
                    }

                    events |= Event.Landed;
                }
            }

            // finalize
            Inputs.Add(input);
            Frame++;
            return events;
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

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "[{0}] ({1})", Y, VSpeed);

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

        public static int Compare(Player a, Player b)
        {
            return a.Frame - b.Frame;
        }
    }
}
