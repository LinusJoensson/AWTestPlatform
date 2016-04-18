using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;

namespace TestPlatform.Repositories
{
    public interface ITestPlatformRepository
    {
        int CreateTest(Test test);
    }
}
