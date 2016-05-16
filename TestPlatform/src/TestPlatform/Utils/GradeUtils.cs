using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;

namespace TestPlatform.Utils
{
    public static class GradeUtils
    {
        public static bool CheckHasPassed(TestSession testSession, int? passPercentage)
        {
            if (!passPercentage.HasValue)
            {
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
