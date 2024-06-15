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
        static double Floor = 408, Ceiling = 363;

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

        public static void SetCeiling(double ceiling)
        {
            Ceiling = Math.Round(ceiling);
        }

        // to make the two ranges disjoint after splitting, one of the ranges needs to be adjusted
        enum BitShift
        {
            UpperRange,
            LowerRange
        }

        PlayerRange2 SplitOffLowerAt(double split, BitShift range)
        {
            // split off lower range
            PlayerRange2 lower = new(StartYLower - YLower + split, StartYLower, split, YLower, VSpeed);

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

        PlayerRange2 SplitOffUpperAt(double split, BitShift range)
        {
            // split off upper range
            PlayerRange2 upper = new(StartYUpper, StartYUpper - YUpper + split, YUpper, split, VSpeed);

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

        public static List<PlayerRange2> FloorCollision(PlayerRange2 p)
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
                    PlayerRange2 lower = p.SplitOffLowerAt(highestCollision, BitShift.UpperRange);
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
                    PlayerRange2 upper = p.SplitOffUpperAt(highestCollisionAfterVspeed, BitShift.UpperRange);
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
                    PlayerRange2 lower = p.SplitOffLowerAt(highestFloorReject, BitShift.UpperRange);
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

        public static List<PlayerRange2> CeilingCollision(PlayerRange2 p)
        {
            double lowestCollision = Ceiling + 0.5;
            if (Math.Round(lowestCollision) > Ceiling)
            {
                lowestCollision = double.BitDecrement(lowestCollision);
            }

            List<PlayerRange2> ranges = [];

            // get stuck range
            // TODO: im not actually sure how i wanna handle ranges stuck in ceiling rn huh
            if (p.YUpper <= lowestCollision)
            {
                if (p.YLower > lowestCollision)
                {
                    // partially stuck
                    PlayerRange2 upper = p.SplitOffUpperAt(lowestCollision, BitShift.LowerRange);
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
                    PlayerRange2 lower = p.SplitOffLowerAt(lowestCollisionAfterVspeed, BitShift.LowerRange);
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
                    PlayerRange2 upper = p.SplitOffUpperAt(lowestCeilingReject, BitShift.LowerRange);
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

        public override string ToString()
        {
            return $"[{StartYUpper}, {StartYLower}] => [{YUpper}, {YLower}] ({VSpeed})";
        }
    }
}
