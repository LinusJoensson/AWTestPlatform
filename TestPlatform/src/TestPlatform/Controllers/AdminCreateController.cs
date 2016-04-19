using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TestPlatform.Repositories;
using TestPlatform.Models;

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

            return RedirectToAction(nameof(ManageTest), new { id = testId } );
        }

        public IActionResult ManageTest(int id)
        {
            return View(repository.GetAllQuestions());
        }

        public IActionResult AddQuestionToTest(int questionId, int testId)
        {
            repository.AddQuestionToTest(questionId, testId);

            return RedirectToAction(nameof(ManageTest), new { id = testId });
        }
    }
}
