using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace old_bruteforcer_rewrite_5
{
    internal class PlayerRange2
    {
        double YUpper, YLower, VSpeed, StartYUpper, StartYLower;
        static double Floor = 408;

        public PlayerRange2(double yUpper, double yLower, double vSpeed)
        {
            if (yUpper > yLower)
            {
                (yUpper, yLower) = (yLower, yUpper);
            }
            YUpper = StartYUpper = yUpper;
            YLower = StartYLower = yLower;
            VSpeed = vSpeed;
        }

        public PlayerRange2(double startYUpper, double startYLower, double yUpper, double yLower, double vSpeed)
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
            VSpeed = vSpeed;
        }

        public static void SetFloor(double floor)
        {
            Floor = Math.Round(floor);
        }

        public PlayerRange2 SplitOffLowerAt(double split, bool bitShiftUpper)
        {
            // split off lower range
            PlayerRange2 lower = new(StartYLower - YLower + split, StartYLower, split, YLower, VSpeed);

            // update current range to exclude split off range (both contain the split point)
            YLower = split;
            StartYLower = StartYUpper - YUpper + split;

            // make disjoint at split
            if (bitShiftUpper)
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

        public PlayerRange2 SplitOffUpperAt(double split, bool bitShiftUpper)
        {
            // split off upper range
            PlayerRange2 upper = new(StartYUpper, StartYUpper - YUpper + split, YUpper, split, VSpeed);

            // update current range to exclude split off range (both contain the split point)
            YUpper = split;
            StartYUpper = StartYLower - YLower + split;

            // make disjoint at split
            if (bitShiftUpper)
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

        public static List<PlayerRange2> test(PlayerRange2 p)
        {
            double highestCollision = Floor - 0.5;
            if (Math.Round(highestCollision) < Floor)
            {
                highestCollision = double.BitIncrement(highestCollision);
            }

            List<PlayerRange2> ranges = [];

            // get stuck range
            if (p.YLower >= highestCollision)
            {
                if (p.YUpper < highestCollision)
                {
                    // partially stuck
                    PlayerRange2 lower = p.SplitOffLowerAt(highestCollision, true);
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
                    PlayerRange2 upper = p.SplitOffUpperAt(highestCollisionAfterVspeed, true);
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

            // the remaining range will land => vspeed = 0
            p.VSpeed = 0;

            // move_contact_solid
            double highestFloorReject = highestCollision - 1;
            while (p.YUpper < highestFloorReject)
            {
                if (p.YLower >= highestFloorReject)
                {
                    PlayerRange2 lower = p.SplitOffLowerAt(highestFloorReject, true);
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

        public override string ToString()
        {
            return $"[{StartYUpper}, {StartYLower}] => [{YUpper}, {YLower}] ({VSpeed})";
        }
    }
}
