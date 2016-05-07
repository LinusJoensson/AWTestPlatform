using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;
using TestPlatform.Models.Enums;
using TestPlatform.Repositories;
using TestPlatform.Utils;
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
            var viewModel = new DashboardVM()
            {
                Tests = model.ToList()
            };
            return View(viewModel);
        }

        [Route("Admin/Test/{testId}/Question/Create")]
        public IActionResult CreateQuestion(int testId)
        {
            int questionId = repository.CreateTestQuestion(testId);

            return RedirectToAction(nameof(UpdateQuestion), new { testId = testId, questionId = questionId });
        }

        public IActionResult UpdateQuestionText(int questionId, string questionText)
        {
            var thisQuestion = repository.GetAllQuestions().SingleOrDefault(o => o.Id == questionId);
            thisQuestion.QuestionText = questionText;

            var model = new QuestionFormVM()
            {
                QuestionText = questionText,
            };

            return PartialView("_EditQuestionPartial", model);
        }

        public IActionResult UpdateAnswer(int answerId, string answerText, bool isCorrect)
        {
            Debug.WriteLine("Size: " + repository.GetAllAnswers().Count());

            foreach (var a in repository.GetAllAnswers())
                Debug.WriteLine("A id: " + a.Id);

            var thisAnswer = repository.GetAllAnswers().SingleOrDefault(o => o.Id == answerId);

            thisAnswer.AnswerText = answerText;
            thisAnswer.IsCorrect = isCorrect;

            var model = new AnswerDetailVM()
            {
                AnswerId = answerId,
                AnswerText = answerText,
                IsChecked = isCorrect
            };

            return PartialView("_EditAnswerPartial", model);
        }

        [Route("Admin/Test/{testId}/Question/{questionId}/Update")]
        public IActionResult UpdateQuestion(int testId, int questionId)
        {
            var thisQuestion = repository.GetAllQuestions().SingleOrDefault(o => o.Id == questionId);

            Debug.WriteLine(thisQuestion.QuestionText);

            var viewModel = new EditQuestionVM()
            {

                ItemType = new SelectListItem[]
                {
                    new SelectListItem { Value = ((int)QuestionType.SingleChoice).ToString(), Text="Single Choice"},
                    new SelectListItem { Value = ((int)QuestionType.MultipleChoice).ToString(), Text="Multiple Choice"},
                    new SelectListItem { Value = ((int)QuestionType.TextSingleLine).ToString(), Text="Single Line Text"},
                    new SelectListItem { Value = ((int)QuestionType.TextMultiLine).ToString(), Text="Multi Line Text"}
                },
                TestId = testId,
                QuestionId = questionId,
                QuestionFormVM = new QuestionFormVM()
                {
                    QuestionText = thisQuestion.QuestionText,
                    QuestionType = thisQuestion.QuestionType,
                    SortOrder = thisQuestion.SortOrder,
                    HasComment = thisQuestion.HasComment,
                },

                AnswerDetailVMs = new List<AnswerDetailVM>()
            };

            foreach (var answer in thisQuestion.Answers)
            {
                viewModel.AnswerDetailVMs.Add(new AnswerDetailVM()
                {
                    AnswerId = answer.Id,
                    AnswerText = answer.AnswerText,
                    ShowAsCorrect = answer.IsCorrect,
                    IsChecked = answer.IsCorrect
                });
            }

            return View(viewModel);

        }

        [Route("Admin/Test/{testId}/Settings")]
        public IActionResult EditTestSettings(int testId)
        {
            var model = repository.GetAllTests()
                .Where(o => o.Id == testId)
                .Select(o => new TestSettingsFormVM
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

        [Route("Admin/Test/{testId}/Settings")]
        [HttpPost]
        public IActionResult EditTestSettings(TestSettingsFormVM viewModel)
        {
            int testId = (int)viewModel.Id;
            var thisTest = repository.GetAllTests().SingleOrDefault(o => o.Id == testId);
            thisTest.Description = viewModel.Description;
            thisTest.Name = viewModel.TestName;

            return RedirectToAction(nameof(AdminController.ManageTestQuestions), new { testId = testId });
        }

        [Route("Admin/Test/Create")]
        public IActionResult CreateTest()
        {
            return View();
        }

        [Route("Admin/Test/Create")]
        [HttpPost]
        public IActionResult CreateTest(TestSettingsFormVM model)
        {
            var testId = repository.CreateTest(new Test()
            {
                Name = model.TestName,
                Description = model.Description
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
            repository.RemoveQuestionFromTest(questionId, testId);
            return RedirectToAction(nameof(ManageTestQuestions), new { testId = testId });
        }

        public IActionResult RemoveAnswer(int testId, int questionId, int answerId)
        {
            //TODO: ARE YOU SURE?
            repository.RemoveAnswerFromQuestion(testId, questionId, answerId);
            return RedirectToAction(nameof(UpdateQuestion), new { testId = testId, questionId = questionId });
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
        public IActionResult CopyQuestionsToTest(int testId, int[] questionIds)
        {
            //TODO: review
            //Add multiple questions in one query (or Json -> Ajax)
            foreach (var qId in questionIds)
                repository.AddQuestionToTest(qId, testId);

            return Json(GetCurrentTestImportData(testId));
        }


        [HttpPost]
        public IActionResult DeleteQuestionsFromTest(int testId, int[] questionIds)
        {
            //TODO: review
            //Add multiple questions in one query (or Json -> Ajax)
            foreach (var qId in questionIds)
                repository.RemoveQuestionFromTest(qId, testId);

            return Json(GetCurrentTestImportData(testId));
        }

        [Route("Admin/Test/{testId}/Question/{questionId}/Answer/Create")]
        public IActionResult CreateAnswer(int testId, int questionId)
        {
            int answerId = repository.CreateAnswer(questionId);

            var viewModelPartial = new AnswerDetailVM()
            {
                AnswerId = answerId
            };

            //TODO: FIX: This updates the whole question (master page) without saving changed question information
            //return RedirectToAction(nameof(UpdateQuestion), new { testId = testId, questionId = questionId });
            return RedirectToAction(nameof(UpdateQuestion), new { testId = testId, questionId = questionId });
        }

        public ActionResult PreviewQuestionPartial(int id)
        {
            var thisQuestion = repository.GetAllQuestions().Single(o => o.Id == id);

            var viewModelPartial = new QuestionFormVM()
            {
                IsInTestSession = false,
                Answers = thisQuestion.Answers.Select(o => new AnswerDetailVM()
                {
                    AnswerId = o.Id,
                    AnswerText = o.AnswerText,
                    ShowAsCorrect = o.IsCorrect,
                    IsChecked = o.IsCorrect
                }).ToList(),
                QuestionText = thisQuestion.QuestionText,
                HasComment = thisQuestion.HasComment,
                QuestionType = thisQuestion.QuestionType
            };

            if (viewModelPartial == null)
                throw new Exception();

            return PartialView("_QuestionFormPartial", viewModelPartial);
        }

        public IActionResult GetImportData(int id)
        {
            var viewModel = new
            {
                allTestsData = GetAllTestsImportData(id),
                currentTestData = GetCurrentTestImportData(id)
            };

            return Json(viewModel);
        }

        object GetAllTestsImportData(int currentTestId)
        {
            var allTests = repository.GetAllTests();

            var allTestsData = allTests.Where(t => t.Id != currentTestId).Select(o => new
            {
                text = o.Name,
                children = o.Questions.Select(q => new
                {
                    id = $"{AppConstants.Import_QuestionIdPrefix}{q.Id}",
                    text = q.QuestionText.Replace("<iframe", "|FRAME|").Replace("<img", "|IMAGE|").Replace("src", "|SOURCE|"),
                    children = q.Answers.Select(a => new
                    {
                        text = $"{a.AnswerText} {(a.IsCorrect ? " (Correct)" : string.Empty)}",
                        state = new { disabled = true }
                    })
                }),
            }).ToArray();

            return allTestsData;
        }

        object GetCurrentTestImportData(int id)
        {
            var allTests = repository.GetAllTests();

            var thisTestData = allTests.Where(o => o.Id == id).Select(o => new
            {
                text = o.Name,
                children = o.Questions.Select(q => new
                {
                    id = $"{AppConstants.Import_QuestionIdPrefix}{q.Id}",
                    text = q.QuestionText.Replace("<iframe", "|FRAME|").Replace("<img", "|IMAGE|").Replace("src", "|SOURCE|"),
                    children = q.Answers.Select(a => new
                    {
                        text = $"{a.AnswerText} {(a.IsCorrect ? " (Correct)" : string.Empty)}",
                        state = new { disabled = true }
                    })
                }),

            }).Single();
            return thisTestData;
        }
    }
}
