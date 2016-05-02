using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.Utils
{
    public static class TimeUtils
    {
        public static int GetSecondsLeft(TimeSpan timeLimit, DateTime StartTime)
        {
            var timeLeft = timeLimit - (DateTime.UtcNow - StartTime);
            return ((int) timeLeft.TotalSeconds);
        }

        public static bool HasTimeLeft(TimeSpan timeLimit, DateTime StartTime)
        {
            return (GetSecondsLeft(timeLimit, StartTime) > 0);
        }

    }
}
