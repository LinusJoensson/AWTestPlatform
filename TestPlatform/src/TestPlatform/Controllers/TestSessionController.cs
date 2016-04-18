using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Repositories;
using TestPlatform.ViewModels;

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
            var viewModel = repository.GetViewQuestion(testSessionId, questionIndex, true);

            return View(viewModel);
        }

        [HttpPost]
        [Route("TestSession/{testSessionId}/{questionIndex}")]
        public IActionResult ViewQuestion(int testSessionId, int questionIndex, QuestionFormVM viewModel, string submit)
        {
            //TODO: update comment and answer for testsession / answer
            if (string.Equals("previous", submit, StringComparison.OrdinalIgnoreCase))
                questionIndex--;
            else if (string.Equals("next", submit, StringComparison.OrdinalIgnoreCase))
                questionIndex++;
            else if (string.Equals("submit", submit, StringComparison.OrdinalIgnoreCase))
            {
                //TODO: update testsession with test submited time (UTCnow)
                //Redirect to new screen
            }
            else
                throw new Exception("Uknown submit value");

            return RedirectToAction(nameof(ViewQuestion), new { TestSessionId = testSessionId, QuestionIndex = questionIndex });
        }
    }
}
