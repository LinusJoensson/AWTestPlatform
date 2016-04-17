using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Repositories;

namespace TestPlatform.Controllers
{
    public class AdminReviewController
    {
        ITestPlatformRepository repository;

        public AdminReviewController(ITestPlatformRepository repository)
        {
            this.repository = repository;
        }
    }
}
