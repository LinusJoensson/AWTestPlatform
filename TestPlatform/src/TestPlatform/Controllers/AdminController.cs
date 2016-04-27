using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;
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
            var model = repository.GetAllTests();
            var dashboard = new DashboardVM
            {
                Tests = model.ToList()
            };
            return View(dashboard);
        }

        [Route("Admin/Test/{testId}/Settings")]
        public IActionResult EditTestSettings(int testId)
        {
            var model = repository.GetAllTests()
                .Where(o => o.Id == testId)
                .Select(o=> new TestSettingsVM
                {
                    Id = o.Id,
                    TestName = o.Name,
                    Description = o.Description,
                    Tags = o.Tags/*,*/
                    //TimeLimit = o.TimeLimit
                })
                .SingleOrDefault();
            return View(model);
        }

        [Route("Admin/Test/Create")]
        public IActionResult CreateTest()
        {
            return View();
        }

        [Route("Admin/Test/Create")]
        [HttpPost]
        public IActionResult CreateTest(TestSettingsVM model)
        {
            var testId = repository.CreateTest(new Test()
            {
                Name = model.TestName
            });

            return RedirectToAction(nameof(AdminController.ManageTestQuestions), new { testId = testId });
        }

        [Route("Admin/Test/{testId}")]
        public IActionResult ManageTestQuestions(int testId)
        {
            var viewModel = repository.GetManageTestQuestionVM(testId);

            return View(viewModel);
        }

        public IActionResult RemoveQuestion(int testId, int questionId)
        {
            //TODO: ARE YOU SURE?
            //Review: remove question from db...?
            repository.RemoveQuestionFromTest(questionId, testId);
            return RedirectToAction(nameof(ManageTestQuestions), new { testId = testId });
        }
         
        [Route("Admin/Test/{testId}/Question/{questionId}")]
        public IActionResult EditQuestion(int testId, int questionId)
        {
            return View();
        }

        [Route("Admin/Test/{testId}/Question/Create")]
        public IActionResult CreateQuestion(int testId)
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
        public IActionResult CopyQuestionsToTest(int testId, int[] questionId)
        {
            //TODO: review
            //Add multiple questions in one query (or Json -> Ajax)
            foreach (var qId in questionId)
                repository.AddQuestionToTest(qId, testId);

            return RedirectToAction(nameof(GetImportData), new { id = testId });
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

        public ActionResult PreviewQuestionPartial(int id)
        {
            var thisQuestion = repository.GetAllQuestions().Single(o => o.Id == id);

            var viewModelPartial = new QuestionFormVM()
            {
                IsInTestSession = true,
                Answers = thisQuestion.Answers.Select(o => new AnswerDetailVM()
                {
                    AnswerId = o.Id,
                    AnswerText = o.AnswerText,
                    ShowAsCorrect = o.IsCorrect,
                    IsChecked = o.IsCorrect
                }).ToList(),
                TextQuestion = thisQuestion.QuestionText,
                HasComment = thisQuestion.HasComment,
                QuestionType = thisQuestion.QuestionType
            };

            if (viewModelPartial == null)
                throw new Exception();


            return PartialView("_QuestionFormPartial", viewModelPartial);
        }
    }
}
