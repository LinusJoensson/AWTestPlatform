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

        [HttpPost]
        public IActionResult CopyQuestionsToTest(int id, int[] questionId)
        {
            //TODO: review
            //Add multiple questions in one query
            foreach (var qId in questionId)
                repository.AddQuestionToTest(qId, id);

            return View();
            //returnera thisTestData
            //Eventuellt: flytta logik till repository
        }

        public IActionResult GetImportData(int id)
        {
            var allTests = repository.GetAllTests();

            var allTestsData = allTests.Select(o => new
            {
                title = o.Name,
                isTestChecked = false,
                tags = o.Tags,
                questionList = o.Questions.Select(q => new
                {
                    questionId = q.Id,
                    questionText = q.QuestionText,
                    answerList = q.Answers.Select(a => new
                    {
                        answerText = a.AnswerText,
                        isCorrect = a.IsCorrect
                    })

                }),

            }).ToArray();

            var thisTestData = allTests.Where(o => o.Id == id).Select(o => new
            {
                title = o.Name,
                isTestChecked = false,
                tags = o.Tags,
                questionList = o.Questions.Select(q => new
                {
                    questionId = q.Id,
                    questionText = q.QuestionText,
                    answerList = q.Answers.Select(a => new
                    {
                        answerText = a.AnswerText,
                        isCorrect = a.IsCorrect
                    })

                }),

            }).Single();

            var viewModel = new
            {
                allTestsData = allTestsData,
                thisTestData = thisTestData
            };

            return Json(viewModel);
        }
    }
}
