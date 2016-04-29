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
            int questionId = repository.CreateQuestion(new Question()
            {
                TestId = testId,
                CreatedDate = DateTime.UtcNow,
            });

            return RedirectToAction(nameof(UpdateQuestion), new { testId = testId, questionId = questionId } );
        }

        [Route("Admin/Test/{testId}/Question/{questionId}/Update")]
        public IActionResult UpdateQuestion(int testId, int questionId)
        {
            var thisQuestion = repository.GetAllQuestions().SingleOrDefault(o => o.Id == questionId);
            
            var viewModel = new EditQuestionFormVM()
            {
                ItemTypes = new SelectListItem[]
                {
                    new SelectListItem { Value = ((int)QuestionType.SingleChoice).ToString(), Text="Single Choice"},
                    new SelectListItem { Value = ((int)QuestionType.MultipleChoice).ToString(), Text="Multiple Choice"},
                    new SelectListItem { Value = ((int)QuestionType.TextSingleLine).ToString(), Text="Single Line Text"},
                    new SelectListItem { Value = ((int)QuestionType.TextMultiLine).ToString(), Text="Multi Line Text"}
                },
                TestId = testId,
                Id = questionId,
                QuestionText = thisQuestion.QuestionText,
                Type = thisQuestion.QuestionType,
                SortOrder = thisQuestion.SortOrder,
                HasComment = thisQuestion.HasComment,
                Answers = new List<AnswerDetailVM>()
            };

            foreach (var answer in thisQuestion.Answers)
            {
                viewModel.Answers.Add(new AnswerDetailVM()
                {
                    AnswerId = answer.Id,
                    AnswerText = answer.AnswerText,
                    ShowAsCorrect = answer.IsCorrect,
                    IsChecked = answer.IsCorrect
                });
            }

            return View(viewModel);
            
        }
        
        [HttpPost]
        [Route("Admin/Test/{testId}/Question/{questionId}/Update")]
        public IActionResult UpdateQuestion(EditQuestionFormVM viewModel, int testId, int questionId)
        {
            var thisQuestion = repository.GetAllQuestions().SingleOrDefault(o => o.Id == questionId);
            thisQuestion.QuestionText = viewModel.QuestionText;
            thisQuestion.QuestionType = viewModel.Type;
            thisQuestion.SortOrder = viewModel.SortOrder;
            thisQuestion.HasComment = viewModel.HasComment;
            
            return RedirectToAction(nameof(UpdateQuestion), new { testId = testId, questionId = questionId });
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
            int testId = (int) viewModel.Id;
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

        
        public ActionResult ListAnswerPartial(int id)
        {
            var thisAnswer = repository.GetAllAnswers().SingleOrDefault(o => o.Id == id);

            var viewModelPartial = new AnswerDetailVM()
            {
                AnswerId = thisAnswer.Id,
                AnswerText = thisAnswer.AnswerText,
                ShowAsCorrect = thisAnswer.IsCorrect,
                IsChecked = thisAnswer.IsCorrect
            };

            return PartialView("_AnswerFormPartial", viewModelPartial);
        }

        [HttpPost]
        public ActionResult EditAnswerPartial(int answerId, string answerText, bool isCorrect)
        {
            var thisAnswer = repository.GetAllAnswers().SingleOrDefault(o => o.Id == answerId);
            thisAnswer.AnswerText = answerText;

            var viewModelPartial = new AnswerDetailVM()
            {
                AnswerId = thisAnswer.Id,
                AnswerText = thisAnswer.AnswerText,
                ShowAsCorrect = thisAnswer.IsCorrect,
                IsChecked = thisAnswer.IsCorrect
            };

            return PartialView("_AnswerFormPartial", viewModelPartial);
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
            return RedirectToAction(nameof(UpdateQuestion), new { testId = testId, questionId = questionId });
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
