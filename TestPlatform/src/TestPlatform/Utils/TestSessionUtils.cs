using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;

namespace TestPlatform.Utils
{
    public class TestSessionUtils
    {
        internal static int GetScore(TestSession ts)
        {
            return 20 * ts.Id;
        }
    }
}
