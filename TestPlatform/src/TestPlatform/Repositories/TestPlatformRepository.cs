using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;
using TestPlatform.Models.Enums;
using TestPlatform.Utils;
using TestPlatform.ViewModels;

namespace TestPlatform.Repositories
{
    public class TestPlatformRepository : ITestPlatformRepository
    {
        public List<Test> _tests { get; set; }
        public List<User> _users { get; set; }
        //public List<Question> _questions { get; set; }
        public List<Answer> _answers { get; set; }
        public List<TestSession> _testSessions { get; set; }
        public List<QuestionResult> _questionResults { get; set; }


        public Question[] GetAllQuestions()
        {
            return (_tests.SelectMany(o => o.Questions)).ToArray();
        }

        public TestPlatformRepository()
        {
            _tests = new List<Test>();
            _users = new List<User>();
            //_questions = new List<Question>();
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

            #region Add static questions 

            var answersCount = _answers.Count();



            #endregion

            _tests.Add(new Test()
            {
                Id = 1,
                //Tags = new List<string>() { "Eazy", "awesome", "heavy" },
                Author = "Linus Joensson",
                Name = "My First Test",
                Description = "An eazy test",
                Questions = new List<Question>()
                {
                    new Question()
                    {
                        Id = GetAllQuestions().Count() + 1,
                        Name = "Third Question",
                        QuestionText = "What notes make up a G major?",
                        QuestionType = QuestionType.SingleChoice,
                        Tags = "Music" + "," + "medium",
                        Author = "Sebastian Uddén",
                        Answers = new List<Answer>()
                        {
                        new Answer() { Id = answersCount + 1, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = true, AnswerText = "G B D" },
                        new Answer() { Id = answersCount + 1, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = false, AnswerText = "G E C" }
                        }
                    }
                },
                TimeLimit = new TimeSpan(0, 10, 0)
            });


            _tests.Add(new Test()
            {
                Id = 2,
                //Tags = new List<string>() { "Eazy", "awesome", "heavy" },
                Author = "Linus Joensson",
                Name = "My Second Test",
                Description = "An eazy test",
                Questions = new List<Question>()
                {
                    new Question()
                    {
                        Id = GetAllQuestions().Count() + 1,
                        Name = "First Question",
                        QuestionText = "Can you answer this?",
                        QuestionType = QuestionType.SingleChoice,
                        Tags = "Music" + "," + "medium",
                        Author = "Sebastian Uddén",
                        Answers = new List<Answer>()
                        {
                        new Answer() { Id = answersCount + 1, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = true, AnswerText = "Gascascas" },
                        new Answer() { Id = answersCount + 
                        1, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = false, AnswerText = "asd asd as " }
                        }
                    },
                    new Question()
                    {
                        Id = GetAllQuestions().Count() + 2,
                        Name = "Second Question",
                        QuestionText = "Another brilliant question, how are you?",
                        QuestionType = QuestionType.SingleChoice,
                        Tags = "Music" + "," + "medium",
                        Author = "Sebastian Uddén",
                        Answers = new List<Answer>()
                        {
                            new Answer() { Id = answersCount + 1, QuestionId = GetAllQuestions().Count() + 2,  IsCorrect = true, AnswerText = "Gascascas" },
                            new Answer() { Id = answersCount + 1, QuestionId = GetAllQuestions().Count() + 2,  IsCorrect = false, AnswerText = "asd asd as " }
                        }
                    },

                },
                TimeLimit = new TimeSpan(0, 10, 0)
            });
        }

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
            });

            return _tests.Last().Id;
        }

        public void AddQuestionToTest(int questionId, int testId)
        {
            var thisTest = _tests.Single(o => o.Id == testId);
            var thisQuestion = GetAllQuestions().Single(o => o.Id == questionId);

            var defaultSortOrder = thisTest.Questions.Count > 0 ?
                thisTest.Questions.Max(o => o.SortOrder) + 100 : 100;

            thisTest.Questions.Add(new Question()
            {
                //Duplicate original question
                Answers = thisQuestion.Answers,
                Name = thisQuestion.Name,
                QuestionType = thisQuestion.QuestionType,
                Tags = thisQuestion.Tags,
                QuestionText = thisQuestion.QuestionText,
                HasComment = thisQuestion.HasComment,

                //New question id
                Id = GetAllQuestions().Count() + 1,

                //Add specific properties
                SortOrder = defaultSortOrder,
                CreatedDate = DateTime.UtcNow,
                Author = _users.ElementAt(0).FirstName,

                //Question belongs to this test
                TestId = testId

            });

        }

        public Test[] GetAllTests() { return _tests.ToArray(); }

        public ViewQuestionVM GetViewQuestion(int testSessionId, int questionIndex, bool isInSession)
        {
            var thisTestSession = _testSessions.Single(o => o.Id == testSessionId);
            var thisTest = _tests.Single(o => o.Id == thisTestSession.TestId);
            var thisQuestion = thisTest.Questions.OrderBy(o => o.SortOrder).ElementAt(questionIndex - 1);
            var thisQuestionResult = thisTestSession.QuestionResults.SingleOrDefault(o => o.QuestionId == thisQuestion.Id);

            //var timeLeft = thisTest.TimeLimit - (DateTime.UtcNow - thisTestSession.StartTime);

            var secondsLeft = TimeUtils.GetSecondsLeft(thisTest.TimeLimit, thisTestSession.StartTime);
            var selectedAnswers = thisQuestionResult?.SelectedAnswers.Split(',');

            return new ViewQuestionVM()
            {
                TestId = thisTest.Id,
                TestTitle = thisTest.Name,
                NumOfQuestion = thisTest.Questions.Count(),
                QuestionIndex = questionIndex,
                SecondsLeft = (int)secondsLeft,

                QuestionFormVM = new QuestionFormVM()
                {
                    IsInTestSession = isInSession,
                    QuestionType = thisQuestion.QuestionType,
                    TextQuestion = thisQuestion.QuestionText,
                    HasComment = thisQuestion.HasComment,
                    Comment = thisQuestionResult?.Comment,
                    SelectedAnswers = selectedAnswers,
                    Answers = thisQuestion.Answers.Select(o => new AnswerDetailVM()
                    {
                        AnswerId = o.Id,
                        AnswerText = o.AnswerText,
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
                TestName = thisTest.Name,
                TimeLimit = thisTest.TimeLimit
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
                UserId = userId,
            });

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

        public bool UpdateSessionAnswers(int testSessionId, int questionIndex, string[] selectedAnswers, string comment)
        {
            var thisTestSession = _testSessions.Single(o => o.Id == testSessionId);
            var thisTest = _tests.Single(o => o.Id == thisTestSession.TestId);
            var thisQuestion = thisTest.Questions.ElementAt(questionIndex - 1);
            var thisQuestionResult = thisTestSession.QuestionResults.Find(o => o.QuestionId == thisQuestion.Id);

            var hasTimeLeft = TimeUtils.HasTimeLeft(thisTest.TimeLimit, thisTestSession.StartTime);

            if (hasTimeLeft)
            {
                if (thisQuestionResult == null)
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
                {
                    foreach (var answer in selectedAnswers)
                        thisQuestionResult.SelectedAnswers += answer + ",";
                }
                if (!string.IsNullOrWhiteSpace(comment))
                    thisQuestionResult.Comment = comment;
            }

            return (hasTimeLeft);
        }

        public void SubmitTestSession(int testSessionId)
        {
            var thisSession = _testSessions.Single(o => o.Id == testSessionId);
            thisSession.SubmitTime = DateTime.UtcNow;
        }

        public TestSession GetTestSessionById(int testSessionId)
        {
            return _testSessions.Single(o => o.Id == testSessionId);
        }

        public void RemoveQuestionFromTest(int questionId, int testId)
        {
            var thisTest = _tests.Single(o => o.Id == testId);
            thisTest.Questions.RemoveAll(o => o.Id == questionId);

        }

        public ManageTestQuestionsVM GetManageTestQuestionVM(int testId)
        {
            var thisTest = _tests.Single(o => o.Id == testId);

            var viewModel = new ManageTestQuestionsVM()
            {
                TestId = testId,
                Description = thisTest.Description,
                Questions = thisTest.Questions,
                TestName = thisTest.Name,
            };

            return viewModel;
        }

        public QuestionFormVM GetPreviewQuestion(int questionId)
        {
            var thisQuestion = GetAllQuestions().Single(o => o.Id == questionId);
            var viewModel = new QuestionFormVM()
            {
                TextQuestion = thisQuestion.QuestionText
            };

            return viewModel;
        }

        public int CreateTestQuestion(int testId)
        {
            var newQuestion = new Question()
            {
                TestId = testId,
                Id = GetAllQuestions().Count() + 1,
                CreatedDate = DateTime.UtcNow,
            };

            _tests.SingleOrDefault(o => o.Id == testId).Questions.Add(newQuestion);

            return newQuestion.Id;
        }

        public int CreateAnswer(int questionId, AnswerDetailVM viewModel)
        {
            var answer = new Answer()
            {
                Id = _answers.Count() + 1,
                AnswerText = viewModel.AnswerText,
                IsCorrect = viewModel.ShowAsCorrect,
                QuestionId = questionId,
            };

            _answers.Add(answer);
            GetAllQuestions().SingleOrDefault(o => o.Id == questionId)?
                .Answers.Add(answer);


            return answer.Id;
        }

        public int CreateAnswer(int questionId)
        {
            var answer = new Answer()
            {
                Id = _answers.Count() + 1
            };

            _answers.Add(answer);
            GetAllQuestions().SingleOrDefault(o => o.Id == questionId)?
                .Answers.Add(answer);

            return answer.Id;
        }

        public void RemoveAnswerFromQuestion(int testId, int questionId, int answerId)
        {
            var thisQuestion = _tests.Single(o => o.Id == testId).Questions.Single(q => q.Id == questionId);
            thisQuestion.Answers.RemoveAll(a => a.Id == answerId);

            _answers.RemoveAll(a => a.Id == answerId);
        }

        public Answer[] GetAllAnswers()
        {
            return _answers.ToArray();
        }

        public SessionCompletedVM GetSessionCompletedVM(int testSessionId, SessionCompletedReason sessionCompletedReason)
        {
            return new SessionCompletedVM()
            {
                IsSuccessfull = true,
                SessionCompletedReason = sessionCompletedReason
            };
        }
    }
}
