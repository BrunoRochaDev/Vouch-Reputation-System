using System;
using System.Collections.Generic;
using System.Text;

namespace VouchReputationSystem
{
    static class Util
    {
        //Function that ensures that values are always between 0 and 1.
        public static float LimitRange(float value, float inclusiveMinimum, float inclusiveMaximum)
        {
            if (value < inclusiveMinimum) { return inclusiveMinimum; }
            if (value > inclusiveMaximum) { return inclusiveMaximum; }
            return value;
        }
    }
}
