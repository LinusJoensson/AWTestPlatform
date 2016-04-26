using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        //Comment: review routing design
        [Route("TestSession/Index/{testId}")]
        public IActionResult Index(int testId)
        {
            var viewModel = repository.GetSessionIndexVM(testId);
            return View(viewModel);
        }

        //Comment: review routing design
        [Route("TestSession/StartSession/{testId}")]
        public IActionResult StartSession(int testId)
        {
            int userId = 1;
            int testSessionId = repository.StartNewSession(userId, testId);

            return RedirectToAction(nameof(ViewQuestion), new { testSessionId = testSessionId, questionIndex = 1 });
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
            repository.UpdateSessionAnswers(testSessionId, questionIndex, viewModel.SelectedAnswers, viewModel.Comment);

            if (string.Equals("previous", submit, StringComparison.OrdinalIgnoreCase))
                questionIndex--;
            else if (string.Equals("next", submit, StringComparison.OrdinalIgnoreCase))
                questionIndex++;
            else if (string.Equals("submit", submit, StringComparison.OrdinalIgnoreCase))
            {
                repository.SubmitTestSession(testSessionId);
                return RedirectToAction(nameof(SubmitSession), new { TestSessionId = testSessionId } );
            }
            else
                throw new Exception("Unknown submit value");

            return RedirectToAction(nameof(ViewQuestion), 
                new { TestSessionId = testSessionId, QuestionIndex = questionIndex });
        }

        [Route("TestSession/{testSessionId}")]
        public IActionResult SubmitSession(int testSessionId)
        {
            var viewModel = repository.GetTestSessionById(testSessionId);
            return View(viewModel);
        }
    }
}
