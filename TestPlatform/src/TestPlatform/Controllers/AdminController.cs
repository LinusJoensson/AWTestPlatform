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

        [Route("Admin/Test/Settings")]
        public IActionResult ManageTestSettings()
        {
            return View();
        }

        [Route("Admin/Test/{testId}")]
        public IActionResult ManageTestQuestions(int testId)
        {
            var viewModel = repository.GetManageTestQuestionVM(testId);

            return View(viewModel);
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

        public IActionResult RemoveQuestion(int testId, int questionId)
        {
            //TODO: ARE YOU SURE?
            //Review: remove question from db...?
            repository.RemoveQuestionFromTest(questionId, testId);
            return RedirectToAction(nameof(ManageTestQuestions), new { testId = testId });
        }

        [Route("Admin/Test/{testId}/Question/{questionId}")]
        public IActionResult ManageQuestion(int testId, int questionId)
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
    }
}
