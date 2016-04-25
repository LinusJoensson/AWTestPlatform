using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Repositories;
using TestPlatform.ViewModels;

namespace TestPlatform.Controllers
{
    public class AdminController : Controller
    {
        ITestPlatformRepository repository;

        public AdminController(ITestPlatformRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [Route("Admin/Test/Create")]
        public IActionResult CreateTest()
        {
            return View();
        }

        [Route("Admin/Test/{testId}")]
        public IActionResult ViewQuestions(int testId)
        {
            return View();
        }

        [Route("Admin/Test/{testId}/Question/{questionId}")]
        public IActionResult CreateQuestion(int testId, int questionId)
        {
            return View();
        }

        [Route("Admin/Test/{testId}/Import")]
        public IActionResult Import(int testId)
        {
            var viewModel = new ImportVM()
            {
                TestId = testId
            };

            return View(viewModel);
        }

        public IActionResult GetAllTests()
        {
            var viewModel = new ImportVM()
            {
                TestId = 1
            };

            return Json(viewModel);
        }
    }
}
