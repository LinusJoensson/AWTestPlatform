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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateTest()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult CreateTest(Test test)
        //{
        //    if (!ModelState.IsValid)
        //        return View(test);

        //    int testId = repository.CreateTest(test);

        //    return RedirectToAction(nameof(EditTestContent), new { testId = testId } );
        //}
        
        //[HttpGet]
        //[Route("AdminCreate/EditTestContent/{testId}")]
        //public IActionResult EditTestContent(int testId)
        //{
        //    var viewModel = repository.GetEditTestContentVM(testId);

        //    return View(viewModel);
        //}
        
    }
}
