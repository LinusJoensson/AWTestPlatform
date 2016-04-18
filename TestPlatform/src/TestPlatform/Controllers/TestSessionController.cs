using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Repositories;

namespace TestPlatform.Controllers
{

    public class TestSessionController : Controller
    {
        ITestPlatformRepository repository;

        public TestSessionController(ITestPlatformRepository repository)
        {
            this.repository = repository;
        }

        
        [Route("TestSession/{testSessionId}/{questionIndex}")]
        public IActionResult ViewQuestion(int testSessionId, int questionIndex)
        {
            var viewModel = repository.GetViewQuestion(testSessionId, questionIndex);
            viewModel.QuestionFormVM.IsInTestSession = true;

            return View(viewModel);
        }

    }
}
