using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TestPlatform.Repositories;
using TestPlatform.Models;
using TestPlatform.ViewModels;

namespace TestPlatform.Controllers
{
    public class AdminCreateController : Controller
    {
        ITestPlatformRepository repository;

        public AdminCreateController(ITestPlatformRepository repository) 
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        // An index from where you can choose to: 
        // - Create an empty test
        // - Create a test from a template
        // - Edit tests
        // - Create test module
        // - Edit module
        public IActionResult TestIndex()
        {
            var viewModel = repository.GetAllTests();
            return View(viewModel);
        }

        public IActionResult CreateTest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTest(Test test)
        {
            if (!ModelState.IsValid)
                return View(test);

            int testId = repository.CreateTest(test);

            return RedirectToAction(nameof(EditTestContent), new { testId = testId } );
        }

        [HttpGet]
        [Route("AdminCreate/EditTestContent/{testId}")]
        public IActionResult EditTestContent(int testId)
        {
            var viewModel = repository.GetEditTestContentVM(testId);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddQuestionsToTest(int testId, string[] selectedItems)
        {
            foreach(var questionId in selectedItems)
                repository.AddQuestionToTest(Convert.ToInt32(questionId), testId);

            return RedirectToAction(nameof(EditTestContent), new { TestId = testId });
        }

        [HttpPost]
        public IActionResult RemoveQuestionsFromTest(int testId, string[] selectedItems)
        {
            foreach (var questionId in selectedItems)
                repository.RemoveQuestionFromTest(Convert.ToInt32(questionId), testId);

            return RedirectToAction(nameof(EditTestContent), new { TestId = testId });
        }
    }
}
