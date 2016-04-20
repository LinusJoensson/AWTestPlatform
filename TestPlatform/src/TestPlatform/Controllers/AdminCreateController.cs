using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TestPlatform.Repositories;
using TestPlatform.Models;
using TestPlatform.ViewModels;
using System.Diagnostics;

namespace TestPlatform.Controllers
{
    public class AdminCreateController : Controller
    {
        ITestPlatformRepository repository;

        public AdminCreateController(ITestPlatformRepository repository) 
        {
            this.repository = repository;
        }

        // An index from where you can choose to: 
        // - Create an empty test
        // - Create a test from a template
        // - Edit tests
        // - Create test module
        // - Edit module
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateTest()
        {
            return View();
        }

        public IActionResult ChooseTestTemplate()
        {
            var viewModel = repository.GetChooseTestTemplateVM();

            return View(viewModel);
        }

        [Route("AdminCreate/CreateTestFromTemplate/{testId}")]
        public IActionResult CreateTestFromTemplate(int testId)
        {
            int newTestId = repository.CreateTestFromTemplate(testId);

            return RedirectToAction(nameof(EditTestContent), new { testId = newTestId });
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

        //NOTE: This action can not take an object reference as input, because the form view model is an interface 
        //Instead each property needs to be explicitly typed as a separate parameter
        //(MVC 6 as of 20/4-2016)
        [HttpPost]
        public IActionResult AddQuestionsToTest(int testId, string[] selectedItems)
        {
            foreach(var questionId in selectedItems)
                repository.AddQuestionToTest(Convert.ToInt32(questionId), testId);

            return RedirectToAction(nameof(EditTestContent), new { testId = testId });
        }

        [HttpPost]
        public IActionResult RemoveQuestionsFromTest(int testId, string[] selectedItems)
        {
            foreach (var questionId in selectedItems)
                repository.RemoveQuestionFromTest(Convert.ToInt32(questionId), testId);

            return RedirectToAction(nameof(EditTestContent), new { testId = testId });
        }
    }
}
