using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rummikub
{
    static class TileLayoutHelper
    {
        //select which row (upper/lower) to use to place a tile in a run
        // return 1 for upper, 0 for lower
        public static int PlaceRun(int[] upper, int[] lower, int X, bool upperJoker, bool lowerJoker)
        {
            /* Heuristic for deciding which row:
 
                  1. Can it complete a block of three? (need to know about 3 tiles to left and right to find this out)
                  2. Can it replace a joker? If so, take the (first/lower) joker
                  3. If only spot is open, take the spot that's open
                  4. If there are adjacent tiles in both spots, put in the spot to give the longer run -- NOT IMPLEMENTED YET.
                  5. Put in the cell with the shortest distance to occupied tile
                  6. Put in the first (lower) spot                     
            */

            int result = 0; //0 for lower result, 1 for upper result
            bool hueristicComplete = false;

            //Can it complete a run of three?
            for (int i = 0; i < 3; i++)
            {
                int start = (X - 3) + i; //start position
                //if the sum of the next 5 occupied cells is exactly 2, it completes a run
                if (start < 0) continue;
                if (lower.Skip(start).Take(5).Sum() == 2)
                {
                    hueristicComplete = true;
                    i = 4;
                }
                else if (upper.Skip(start).Take(5).Sum() == 2)
                {
                    result = 1;
                    hueristicComplete = true;
                    i = 4;
                }
            }

            //Can it replace a joker?
            if (!hueristicComplete)
            {
                if (lowerJoker)
                {
                    hueristicComplete = true;
                }
                else if (upperJoker)
                {
                    result = 1;
                    hueristicComplete = true;
                }
            }

            // Is only one spot open?
            if (!hueristicComplete && lower[X] == 1 ^ upper[X] == 1)
            {
                result = upper[X];
                hueristicComplete = true;
            }

            // At this point, we know both spots are open (did not replace a joker, not just one spot open, this is not a joker)
            // It will be convenient for processing further rules to assume the spot will be filled
            lower[X] = 1;
            upper[X] = 1;

            // If there are adjacent tiles in both spots, put in the spot to give the longer run 
            if (!hueristicComplete)
            {
                int lowerTotal = 0, upperTotal = 0;//count my tile
                int position = X;
                while (position >= 0 && upper[position] == 1) { upperTotal++; position--; }
                position = X + 1;
                while (position < upper.Length && upper[position] == 1) { upperTotal++; position++; }
                position = X;
                while (position >= 0 && lower[position] == 1) { lowerTotal++; position--; }
                position = X + 1;
                while (position < lower.Length && lower[position] == 1) { lowerTotal++; position++; }

                if (lowerTotal > upperTotal)
                {
                    hueristicComplete = true;
                }
                else if (upperTotal > lowerTotal)
                {
                    hueristicComplete = true;
                    result = 1;
                }
            }

            //Put in the cell with the shortest distance to occupied tile
            if (!hueristicComplete)
            {
                int lowerTotalLeft = 0, lowerTotalRight=0, upperTotalLeft = 0, upperTotalRight = 0;
                int position = X - 1;
                while (position >= 0 && upper[position] == 0) { upperTotalLeft++; position--; }
                if (position < 0) upperTotalLeft = 50; //only count if we actually found a tile
                position = X + 1;
                while (position < upper.Length && upper[position] == 0) { upperTotalRight++; position++; }
                if (position >= upper.Length) upperTotalRight = 50;
                position = X - 1;
                while (position >= 0 && lower[position] == 0) { lowerTotalLeft++; position--; }
                if (position < 0) lowerTotalLeft = 50;
                position = X + 1;
                while (position < lower.Length && lower[position] == 0) { lowerTotalRight++; position++; }
                if (position >= lower.Length) lowerTotalRight = 50;

                if (lowerTotalRight < lowerTotalLeft) lowerTotalLeft = lowerTotalRight;
                if (upperTotalRight < upperTotalLeft) upperTotalLeft = upperTotalRight;
                if (lowerTotalLeft < upperTotalLeft)
                {
                    hueristicComplete = true;
                }
                else if (upperTotalLeft < lowerTotalLeft)
                {
                    hueristicComplete = true;
                    result = 1;
                }
            }
            //lower by default
            return result;
        }
    }
}
