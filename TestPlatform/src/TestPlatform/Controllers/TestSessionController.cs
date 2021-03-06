﻿using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models.Enums;
using TestPlatform.Repositories;
using TestPlatform.Utils;
using TestPlatform.ViewModels;

namespace TestPlatform.Controllers
{

    public class TestSessionController : Controller
    {
        ITestPlatformRepository repository;

        public TestSessionController(ITestPlatformRepository repository)
        {
            this.repository = repository;
        }

        //Comment: review routing design
        [Route("TestSession/Index/{testId}")]
        public IActionResult Index(int testId)
        {
            var viewModel = repository.GetSessionIndexVM(testId);
            return View(viewModel);
        }

        //Comment: review routing design
        [Route("TestSession/StartSession/{testId}")]
        public IActionResult StartSession(int testId)
        {
            int userId = 1;
            int testSessionId = repository.StartNewSession(userId, testId);

            return RedirectToAction(nameof(ViewQuestion), new { testSessionId = testSessionId, questionIndex = 1 });
        }

        [Route("TestSession/{testSessionId}/{questionIndex}")]
        public IActionResult ViewQuestion(int testSessionId, int questionIndex)
        {
            var viewModel = repository.GetViewQuestion(testSessionId, questionIndex, true);
            if (viewModel.SecondsLeft.HasValue && viewModel.SecondsLeft <= 0)
            {
                // Timed out
                return RedirectToAction(nameof(SessionCompleted),
                    new { TestSessionId = testSessionId, completedReason = (int)SessionCompletedReason.TimedOut });
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("TestSession/{testSessionId}/{questionIndex}")]
        public IActionResult ViewQuestion(int testSessionId, int questionIndex, QuestionFormVM viewModel, string submit)
        {
            var hasTimeLeft = repository.UpdateSessionAnswers(testSessionId, questionIndex, viewModel.SelectedAnswers, viewModel.Comment);
            var actionAndSecondsFromButton = submit.Split(' ');
            double secondsLeft;
            string submitAction = actionAndSecondsFromButton[0];  
            if (double.TryParse(actionAndSecondsFromButton[1], out secondsLeft))

            if (string.Equals("previous", submitAction, StringComparison.OrdinalIgnoreCase))
                questionIndex--;
            else if (string.Equals("next", submitAction, StringComparison.OrdinalIgnoreCase))
                questionIndex++;
            else if (string.Equals("submit", submitAction, StringComparison.OrdinalIgnoreCase))
            {
                repository.SubmitTestSession(testSessionId);
                return RedirectToAction(nameof(SessionCompleted),
                    new { TestSessionId = testSessionId, completedReason = (int)SessionCompletedReason.Completed });
            }
            else
                throw new Exception("Unknown submit value");

            if (hasTimeLeft)
            {
                var session = repository.GetTestSessionById(testSessionId);
                session.SecondsLeft = secondsLeft;

                return RedirectToAction(nameof(ViewQuestion),
                    new { TestSessionId = testSessionId, QuestionIndex = questionIndex });
            }
            else
            {
                return RedirectToAction(nameof(SessionCompleted),
                    new { TestSessionId = testSessionId, completedReason = (int)SessionCompletedReason.TimedOut });
            }
        }

        [Route("SessionCompleted/{testSessionId}/{completedReason}")]
        public IActionResult SessionCompleted(int testSessionId, int completedReason)
        {
            var viewModel = repository.GetSessionCompletedVM(testSessionId, (SessionCompletedReason)completedReason);
            return View(viewModel);
        }

    }
}
