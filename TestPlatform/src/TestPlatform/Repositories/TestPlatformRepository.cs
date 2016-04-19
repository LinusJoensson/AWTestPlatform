﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;
using TestPlatform.Models.Enums;
using TestPlatform.ViewModels;

namespace TestPlatform.Repositories
{
    public class TestPlatformRepository : ITestPlatformRepository
    {
        public List<Test> _tests { get; set; }
        public List<User> _users { get; set; } 
        public List<Question> _questions { get; set; }
        public List<Answer> _answers { get; set; }
        public List<QuestionCategory> _questionCategories { get; set; }
        public List<TestCategory> _testCategories { get; set; }
        public List<TestSession> _testSessions { get; set; }
        public List<QuestionResult> _questionResults { get; set; }

        public TestPlatformRepository()
        {
            _tests = new List<Test>();
            _users = new List<User>();
            _questions = new List<Question>();
            _questionCategories = new List<QuestionCategory>();
            _testCategories = new List<TestCategory>();
            _answers = new List<Answer>();
            _testSessions = new List<TestSession>();
            _questionResults = new List<QuestionResult>();

            _testSessions.Add(new TestSession()
            {
                Id = 1,
                TestId = 1,
                UserId = 1,
                QuestionResults = new List<QuestionResult>(),
                StartTime = DateTime.UtcNow,
            });

            #region Add static users
            _users.Add(new User()
            {
                Id = 1,
                Email = "linus.joensson.ms@outlook.com",
                FirstName = "Linus",
                Lastname = "Joensson",
                TestSessions = new List<TestSession>()
            });
            _users.Last().TestSessions.Add(_testSessions.Last());
            #endregion

            #region Add static categories 
            _questionCategories.Add(new QuestionCategory()
            {
                Id = 1,
                Name = "My First Question Category",
                //Tags = new List<string>() { "first", "good questions" },
                Author = "Linus Joensson",
                Description = "Very good questions",
            });

            _testCategories.Add(new TestCategory()
            {
                Id = 1,
                Name = "My First Test Category",
                //Tags = new List<string>() { "first", "good tests" },
                Author = "Linus Joensson",
                Description = "Very good tests",
            });

            #endregion

            #region Add static questions 

            var answersCount = _answers.Count();
            _questions.Add(new Question()
            {
                Id = 1,
                Name = "First Question",
                TextQuestion = "Is this the first question?",
                QuestionType = QuestionType.SingleChoice,
                Tags = "for lolz" + "," + "eazy",
                Author = "Linus Joensson",
                Category = _questionCategories.First(),
                HasComment = true,
                Answers = new List<Answer>()
                {
                    new Answer() { Id = answersCount + 1, QuestionId = 1,  IsCorrect = true, TextAnswer = "Yes" },
                    new Answer() { Id = answersCount + 2, QuestionId = 1,  IsCorrect = false, TextAnswer = "Yes! ... Uhm I mean no" }
                }
            });

            _answers.Add(_questions.Last().Answers.ElementAt(0));
            _answers.Add(_questions.Last().Answers.ElementAt(1));

            answersCount = _answers.Count();
            _questions.Add(new Question()
            {
                Id = 2,
                Name = "Second Question",
                TextQuestion = "Is this the third question?",
                QuestionType = QuestionType.MultipleChoice,
                Tags = "for lolz" + "," + "eazy",
                Author = "Linus Joensson",
                Category = _questionCategories.First(),
                HasComment = true,
                Answers = new List<Answer>()
                {
                    new Answer() { Id = answersCount + 1, QuestionId = 2,  IsCorrect = false, TextAnswer = "Yes" },
                    new Answer() { Id = answersCount + 2, QuestionId = 2,  IsCorrect = true, TextAnswer = "Yes! ... Uhm I mean no" }
                }
            });
            _answers.Add(_questions.Last().Answers.ElementAt(0));
            _answers.Add(_questions.Last().Answers.ElementAt(1));

            answersCount = _answers.Count();
            _questions.Add(new Question()
            {
                Id = 3,
                Name = "Second Question",
                TextQuestion = "Is THIS the third question?",
                QuestionType = QuestionType.SingleChoice,
                Tags = "for lolz" + "," + "hard",
                Author = "Linus Joensson",
                Category = _questionCategories.First(),
                Answers = new List<Answer>()
                {
                    new Answer() { Id = answersCount + 1, QuestionId = 3,  IsCorrect = true, TextAnswer = "Yes" },
                    new Answer() { Id = answersCount + 2, QuestionId = 3,  IsCorrect = false, TextAnswer = "Yes! ... Uhm I mean no" }
                }
            });
            _answers.Add(_questions.Last().Answers.ElementAt(0));
            _answers.Add(_questions.Last().Answers.ElementAt(1));

            #endregion

            #region Add static tests
            _tests.Add(new Test()
            {
                Id = 1,
                //Tags = new List<string>() { "Eazy", "awesome", "heavy" },
                Author = "Linus Joensson",
                Category = _testCategories.ElementAt(0),
                Name = "My First Test",
                Description = "An eazy test",
                Questions = new List<Question>(),
            });
            #endregion

            #region Add static questions to test

            //  Select and add questions just as would be done in action when a test is created from scratch:
            //
            // 1) User selects a question to add to the test
            // 2) The question is collected from the questions table in the database
            // 3) The collected question is added to the test as a duplicate object
            // *  The Duplicated question belongs to the test and may now have test-specific properties such as SortOrder
            // *  The Original question is left untouched in the questions table 

            var selectedQuestionId = 1;
            var selectedQuestion = _questions.Find(o => o.Id == selectedQuestionId);
            var selectedTest = _tests.ElementAt(0);

            if (selectedQuestion == null)
                throw new Exception("The question id " + selectedQuestionId + " does not exist in database");

            //Note that C# does not support object duplication so we need to do this manually
            selectedTest.Questions.Add(new Question()
            {
                //Duplication
                Answers = selectedQuestion.Answers,
                Tags = selectedQuestion.Tags,
                Author = selectedQuestion.Author,
                Category = selectedQuestion.Category,
                Name = selectedQuestion.Name,
                QuestionType = selectedQuestion.QuestionType,
                TextQuestion = selectedQuestion.TextQuestion,
                HasComment = selectedQuestion.HasComment,

                //Test specific properties
                Id = _questions.Count() + 1,
                TestId = _tests.Last().Id,
                SortOrder = 100
            });

            //We use the same database table for questions owned by tests as for questions without owner
            _questions.Add(selectedTest.Questions.Last());


            selectedQuestionId = 2;
            selectedQuestion = _questions.Find(o => o.Id == selectedQuestionId);
            selectedTest = _tests.ElementAt(0);

            if (selectedQuestion == null)
                throw new KeyNotFoundException("The question id " + selectedQuestionId + " does not exist in database");

            //Note that C# does not support object duplication so we need to do this manually
            selectedTest.Questions.Add(new Question()
            {
                //Duplication
                Answers = selectedQuestion.Answers,
                Tags = selectedQuestion.Tags,
                Author = selectedQuestion.Author,
                Category = selectedQuestion.Category,
                Name = selectedQuestion.Name,
                QuestionType = selectedQuestion.QuestionType,
                TextQuestion = selectedQuestion.TextQuestion,
                HasComment = selectedQuestion.HasComment,

                //Test specific properties
                Id = _questions.Count() + 1,
                TestId = _tests.Last().Id,
                SortOrder = 200
            });

            //We use the same database table for questions owned by tests as for questions without owner
            _questions.Add(selectedTest.Questions.Last());

            
        }

        #endregion

        public int CreateTest(Test test)
        {
            _tests.Add(new Test()
            {
                //Dynamic
                Id = _tests.Count + 1,
                Description = test.Description,
                Questions = new List<Question>(),
                Name = test.Name,

                //Static
                IsPublished = true,
                //Tags = new List<string>() { "happy", "insane" },
                Author = _users.ElementAt(0).FirstName,
                Category = _testCategories.ElementAt(0),
            });

            return _tests.Last().Id;  
        }

        public void AddQuestionToTest(int questionId, int testId)
        {
            var thisTest = _tests.Single(o => o.Id == testId);
            var thisQuestion = _questions.Single(o => o.Id == questionId);

            var defaultSortOrder = thisTest.Questions.Count > 0 ?
                thisTest.Questions.Max(o => o.SortOrder) + 100 : 100;

            thisTest.Questions.Add(new Question()
            {
                //Duplicate original question
                Answers = thisQuestion.Answers,
                Category = thisQuestion.Category,
                Name = thisQuestion.Name,
                QuestionType = thisQuestion.QuestionType,
                Tags = thisQuestion.Tags,
                TextQuestion = thisQuestion.TextQuestion,
                HasComment = thisQuestion.HasComment,

                //New question id
                Id = _questions.Count() + 1,

                //Add specific properties
                SortOrder = defaultSortOrder,
                CreatedDate = DateTime.UtcNow,
                Author = _users.ElementAt(0).FirstName,

                //Question belongs to this test
                TestId = testId

            });

            //Add to questions list
            _questions.Add(thisTest.Questions.Last());
        }

        #region Get All Objects
        public Test[] GetAllTests() { return _tests.ToArray(); }
        public Question[] GetAllQuestions() { return _questions.ToArray(); }


        #endregion

        public ViewQuestionVM GetViewQuestion(int testSessionId, int questionIndex, bool isInSession)
        {
            var thisTestSession = _testSessions.Single(o => o.Id == testSessionId);
            var thisTest = _tests.Single(o => o.Id == thisTestSession.TestId);
            var thisQuestion = thisTest.Questions.OrderBy(o => o.SortOrder).ElementAt(questionIndex - 1);
            var thisQuestionResult = thisTestSession.QuestionResults.SingleOrDefault(o => o.QuestionId == thisQuestion.Id);

            var timeLeft = (2*60*1000 - (DateTime.UtcNow - thisTestSession.StartTime).TotalMilliseconds)/(60*100);
            var selectedAnswers = thisQuestionResult?.SelectedAnswers.Split(',');

            return new ViewQuestionVM()
            {
                TestTitle = thisTest.Name,
                NumOfQuestion = thisTest.Questions.Count(),
                QuestionIndex = questionIndex,
                TimeLeft = (int)timeLeft,

                QuestionFormVM = new QuestionFormVM()
                {
                    IsInTestSession = isInSession,
                    QuestionType = thisQuestion.QuestionType,
                    TextQuestion = thisQuestion.TextQuestion,
                    HasComment = thisQuestion.HasComment,
                    Comment = thisQuestionResult?.Comment,
                    SelectedAnswers = selectedAnswers,
                    Answers = thisQuestion.Answers.Select(o => new AnswerDetailVM()
                    {
                        AnswerId = o.Id,
                        AnswerText = o.TextAnswer,
                        ShowAsCorrect = ((!isInSession) && (o.IsCorrect)),
                        IsChecked = selectedAnswers == null ? false :
                            ((isInSession) && (selectedAnswers.Contains(o.Id.ToString()))),

                    }).ToList()
                }
            };
        }

        public SessionIndexVM GetSessionIndexVM(int testId)
        {
            var thisTest = _tests.Single(o => o.Id == testId);
            var thisUserId = 1;

            var viewModel = new SessionIndexVM()
            {
                UserId = thisUserId,
                TestId = thisTest.Id,
                NumberOfQuestions = thisTest.Questions.Count(),
                TestDescription = thisTest.Description,
                TestName = thisTest.Name
            };

            return viewModel;
        }

        public int StartNewSession(int userId, int testId)
        {
            var thisUser = _users.Single(o => o.Id == userId);
            var thisTest = _tests.Single(o => o.Id == testId);

            thisUser.TestSessions.Add(new TestSession()
            {
                Id = _testSessions.Count() + 1,
                QuestionResults = new List<QuestionResult>(),
                StartTime = DateTime.UtcNow,
                TestId = testId,
                UserId = userId
            });

            // Allocate space for question results right away
            // To be able to easily display information such as if every question have been answered etc
            for (int i = 1; i <= thisTest.Questions.Count(); i++)
                thisUser.TestSessions.Last().QuestionResults.Add(new QuestionResult()
                {
                    Id = _questionResults.Count() + i,
                    QuestionId = thisTest.Questions.ElementAt(i - 1).Id,
                    SelectedAnswers = "",
                });

            _testSessions.Add(thisUser.TestSessions.Last());

            return _testSessions.Last().Id;
        }

        public void UpdateSessionAnswers(int testSessionId, int questionIndex, string[] selectedAnswers, string comment)
        {
            var thisTestSession = _testSessions.Single(o => o.Id == testSessionId);
            var thisTest = _tests.Single(o => o.Id == thisTestSession.TestId);
            var thisQuestion = thisTest.Questions.ElementAt(questionIndex - 1);
            var thisQuestionResult = thisTestSession.QuestionResults.Find(o => o.QuestionId == thisQuestion.Id);

            if(thisQuestionResult == null)
            {
                thisTestSession.QuestionResults.Add(new QuestionResult()
                {
                    Id = _questionResults.Count() + 1,
                    QuestionId = thisQuestion.Id,
                });
                _questionResults.Add(thisTestSession.QuestionResults.Last());
                thisQuestionResult = thisTestSession.QuestionResults.Last();
            }

            thisQuestionResult.SelectedAnswers = "";

            if (selectedAnswers != null)
                foreach (var answer in selectedAnswers)
                    thisQuestionResult.SelectedAnswers = answer + ",";

            if (!string.IsNullOrWhiteSpace(comment))
                thisQuestionResult.Comment = comment;
        }

        public void SubmitTestSession(int testSessionId)
        {
            var thisSession = _testSessions.Single(o => o.Id == testSessionId);
            thisSession.SubmitTime = DateTime.UtcNow;
        }

        public TestSession GetTestSessionById(int testSessionId)
        {
            return _testSessions.Single(o => o.Id == testSessionId); ;
        }
    }
}
