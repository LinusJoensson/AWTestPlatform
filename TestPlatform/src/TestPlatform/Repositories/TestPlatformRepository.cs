using Microsoft.AspNet.Mvc.Rendering;
using Newtonsoft.Json;
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

        public Answer[] GetAllAnswers()
        {
            return (_tests.SelectMany(o => o.Questions).SelectMany(q => q.Answers)).ToArray();
        }

        public TestPlatformRepository()
        {
            _tests = new List<Test>();
            _users = new List<User>();
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
            _users.Add(new User()
            {
                Id = 2,
                Email = "sebastian.udden@gmail.com",
                FirstName = "Sebastian",
                Lastname = "Uddén",
                TestSessions = new List<TestSession>()
            });
            _users.Add(new User()
            {
                Id = 3,
                Email = "mattiashagelin@outlook.com",
                FirstName = "Mattias",
                Lastname = "Hagelin",
                TestSessions = new List<TestSession>()
            });
            _users.Add(new User()
            {
                Id = 4,
                Email = "patrikweibus@outlook.com",
                FirstName = "Patrik",
                Lastname = "Weibus",
                TestSessions = new List<TestSession>()
            });
            _users.Last().TestSessions.Add(_testSessions.Last());
            #endregion

            #region Add static tests
            _tests.Add(new Test()
            {

                Id = 1,
                //Tags = new List<string>() { "Eazy", "awesome", "heavy" },
                Author = "Linus Joensson",
                Name = "Basic C#",
                Description = "An eazy test",
                Questions = new List<Question>()
                {
                    new Question()
                    {
                        TestId = 1,
                        Id = 1,
                        Name = "First Question",
                        QuestionText = /*@"<iframe src=""//www.youtube.com/embed/ncclpqQzjY0"" width=""auto"" height=""auto"" allowfullscreen=""allowfullscreen""></iframe>"*/"How do you write into console.",
                        QuestionType = QuestionType.SingleChoice,
                        Tags = "C#" + "," + "hard",
                        Author = "Sebastian Uddén",
                        Answers = new List<Answer>()
                        {
                        new Answer() { Id = 1, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = true, AnswerText = "Console.WriteLine()" },
                        new Answer() { Id = 2, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = false, AnswerText = "Console.ReadLine()" }
                        }
                    },
                    new Question()
                    {
                        TestId = 1,
                        Id = 2,
                        Name = "Second Question",
                        QuestionText = "What is the meaning of life?",
                        QuestionType = QuestionType.SingleChoice,
                        Tags = "Life" + "," + "medium",
                        Author = "Sebastian Uddén",
                        Answers = new List<Answer>()
                        {
                        new Answer() { Id = 3, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = false, AnswerText = "I don't know, death?" },
                        new Answer() { Id = 4, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = true, AnswerText = "42" }
                        }
                    }
                },
                TimeLimitInMinutes = 10,
                TestSessions = new List<TestSession>()
            });

            _tests[0].TestSessions.Add(
                    new TestSession
                    {
                        Id = 1,
                        TestId = 1,
                        UserId = 1,
                        QuestionResults = new List<QuestionResult>
                        {
                            new QuestionResult
                            {
                                Id = 1,
                                Comment = null,
                                QuestionId = 1,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 1),
                                SelectedAnswers = "2"
                            },
                            new QuestionResult
                            {
                                Id = 2,
                                Comment = null,
                                QuestionId = 2,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 2),
                                SelectedAnswers = "3"
                            }
                        },
                        StartTime = DateTime.Now,
                        SubmitTime = DateTime.Now.AddMinutes(10),
                        User = _users.Single(o => o.Id == 1)
                    });

            _tests[0].TestSessions.Add(
                new TestSession
                {
                    Id = 2,
                    TestId = 1,
                    UserId = 2,
                    QuestionResults = new List<QuestionResult>
                        {
                            new QuestionResult
                            {
                                Id = 3,
                                Comment = null,
                                QuestionId = 1,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 1),
                                SelectedAnswers = "1"
                            },
                            new QuestionResult
                            {
                                Id = 4,
                                Comment = null,
                                QuestionId = 2,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 2),
                                SelectedAnswers = "4"
                            }
                        },
                    StartTime = DateTime.Now,
                    SubmitTime = DateTime.Now.AddMinutes(8),
                    User = _users.Single(o => o.Id == 2)
                });
            _tests[0].TestSessions.Add(
                new TestSession
                {
                    Id = 3,
                    TestId = 1,
                    UserId = 3,
                    QuestionResults = new List<QuestionResult>
                        {
                            new QuestionResult
                            {
                                Id = 5,
                                Comment = null,
                                QuestionId = 1,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 1),
                                SelectedAnswers = "1"
                            },
                            new QuestionResult
                            {
                                Id = 6,
                                Comment = null,
                                QuestionId = 2,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 2),
                                SelectedAnswers = "3"
                            }
                        },
                    StartTime = DateTime.Now,
                    SubmitTime = DateTime.Now.AddMinutes(1),
                    User = _users.Single(o => o.Id == 3)
                });
            _tests[0].TestSessions.Add(
                new TestSession
                {
                    Id = 4,
                    TestId = 1,
                    UserId = 4,
                    QuestionResults = new List<QuestionResult>
                        {
                            new QuestionResult
                            {
                                Id = 7,
                                Comment = null,
                                QuestionId = 1,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 1),
                                SelectedAnswers = "2"
                            },
                            new QuestionResult
                            {
                                Id = 8,
                                Comment = null,
                                QuestionId = 2,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 2),
                                SelectedAnswers = "4"
                            }
                        },
                    StartTime = DateTime.Now,
                    SubmitTime = DateTime.Now.AddMinutes(30),
                    User = _users.Single(o => o.Id == 4)
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
                        Name = "<p>What is a variable?</p>",
                        QuestionText = "What is a variable?",
                        QuestionType = QuestionType.MultipleChoice,
                        Tags = "Music" + "," + "medium",
                        Author = "Sebastian Uddén",
                        Answers = new List<Answer>()
                        {
                        new Answer() { Id = GetAllAnswers().Count() + 1, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = true, AnswerText = "A store of value" },
                        new Answer() { Id = GetAllAnswers().Count() + 2, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = false, AnswerText = "A banana " },
                        new Answer() { Id = GetAllAnswers().Count() + 3, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = false, AnswerText = "All of the above " },
                        }
                    },
                    new Question()
                    {
                        Id = GetAllQuestions().Count() + 2,
                        Name = "Second Question",
                        QuestionText = @"<iframe src=""//www.youtube.com/embed/ncclpqQzjY0"" width=""560"" height=""314"" allowfullscreen=""allowfullscreen""></iframe>",
                        QuestionType = QuestionType.SingleChoice,
                        Tags = "Music" + "," + "medium",
                        Author = "Sebastian Uddén",
                        Answers = new List<Answer>()
                        {
                            new Answer() { Id = GetAllAnswers().Count() + 4, QuestionId = GetAllQuestions().Count() + 2,  IsCorrect = true, AnswerText = "Zlatan" },
                            new Answer() { Id = GetAllAnswers().Count() + 5, QuestionId = GetAllQuestions().Count() + 2,  IsCorrect = false, AnswerText = "Zlatan" }
                        }
                    },

                },
                TimeLimitInMinutes = null
            });
            #endregion
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
                Tags = test.Tags,
                ShowPassOrFail = test.ShowPassOrFail,
                ShowTestScore = test.ShowTestScore,
                CertTemplatePath = test.CertTemplatePath,
                CustomCompletionMessage = test.CustomCompletionMessage,
                TimeLimitInMinutes = test.TimeLimitInMinutes,
                PassPercentage = test.PassPercentage,
                EnableCertDownloadOnCompletion = test.EnableCertDownloadOnCompletion,
                EnableEmailCertOnCompletion = test.EnableEmailCertOnCompletion,

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

            //var secondsLeft = TimeUtils.GetSecondsLeft(thisTest.TimeLimitInMinutes, thisTestSession.StartTime);
            var selectedAnswers = thisQuestionResult?.SelectedAnswers.Split(',');

            return new ViewQuestionVM()
            {
                TestId = thisTest.Id,
                TestTitle = thisTest.Name,
                NumOfQuestion = thisTest.Questions.Count(),
                QuestionIndex = questionIndex,
                SecondsLeft = thisTest.TimeLimitInMinutes.HasValue ? thisTest.TimeLimitInMinutes * 60 : null,

                QuestionFormVM = new QuestionFormVM()
                {
                    IsInTestSession = isInSession,
                    QuestionType = thisQuestion.QuestionType,
                    QuestionText = thisQuestion.QuestionText,
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
                TimeLimit = thisTest.TimeLimitInMinutes
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

            var hasTimeLeft = TimeUtils.HasTimeLeft(thisTest.TimeLimitInMinutes, thisTestSession.StartTime);

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
                Questions = thisTest.Questions.OrderBy(o => o.SortOrder).ToList(),
                TestName = thisTest.Name,
            };

            return viewModel;
        }

        public QuestionFormVM GetPreviewQuestion(int questionId)
        {
            var thisQuestion = GetAllQuestions().Single(o => o.Id == questionId);
            var viewModel = new QuestionFormVM()
            {
                QuestionText = thisQuestion.QuestionText
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
                Id = GetAllAnswers().Count() + 1,
                AnswerText = viewModel.AnswerText,
                IsCorrect = viewModel.ShowAsCorrect,
                QuestionId = questionId,
            };

            GetAllQuestions().SingleOrDefault(o => o.Id == questionId)?
                .Answers.Add(answer);

            return answer.Id;
        }

        public int CreateAnswer(int questionId)
        {
            var answer = new Answer()
            {
                Id = GetAllAnswers().Count() + 1
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

        public SessionCompletedVM GetSessionCompletedVM(int testSessionId, SessionCompletedReason sessionCompletedReason)
        {
            var user = _users.Single(u => _testSessions.Single(o => o.Id == testSessionId).UserId == u.Id);
            return new SessionCompletedVM()
            {
                Date = DateTime.Now.Date.ToString("dd/MM/yyyy"),
                IsSuccessfull = true,
                UserName = $"{user.FirstName} {user.Lastname}",
                SessionCompletedReason = sessionCompletedReason
            };
        }

        public EditQuestionVM GetEditQuestionVM(int testId, int questionId)
        {
            var thisQuestion = GetAllQuestions().SingleOrDefault(o => o.Id == questionId);

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
                Type = thisQuestion.QuestionType,
                SortOrder = thisQuestion.SortOrder,
                QuestionFormVM = new QuestionFormVM()
                {
                    QuestionText = thisQuestion.QuestionText,
                    QuestionType = thisQuestion.QuestionType,
                    SortOrder = thisQuestion.SortOrder,
                    HasComment = thisQuestion.HasComment,
                    IsInEditQuestion = true
                },
                HasComment = thisQuestion.HasComment,
            };

            if ((viewModel.Type == QuestionType.MultipleChoice) || (viewModel.Type == QuestionType.SingleChoice))
            {
                viewModel.AnswerDetailVMs = thisQuestion.Answers.Select(o => new AnswerDetailVM()
                {
                    AnswerId = o.Id,
                    AnswerText = o.AnswerText,
                    IsChecked = o.IsCorrect,
                    ShowAsCorrect = o.IsCorrect,
                    QuestionType = thisQuestion.QuestionType,
                }).ToArray();
            }

            return (viewModel);
        }

        public QuestionFormVM GetPreviewQuestionPartial(int questionId)
        {
            var thisQuestion = GetAllQuestions().Single(o => o.Id == questionId);

            var viewModel = new QuestionFormVM()
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

            return viewModel;

        }

        public ShowResultsVM GetShowResultsVM(int testId)
        {
            var test = _tests.Single(o => o.Id == testId);
            var maxScore = test.Questions.Count();
            var result = new
            {
                resultData = new
                {
                    maxScore = maxScore,
                    passResult = 0.5 * maxScore
                },
                students = test.TestSessions
                .Select(ts => new
                {
                    name = ts.User.FirstName,
                    email = ts.User.Email,
                    testscore = TestSessionUtils.GetScore(ts,GetAllAnswers(), GetAllQuestions())
                }).ToArray()
            };
            return new ShowResultsVM
            {
                ResultDataJSON = JsonConvert.SerializeObject(result)
            };
        }
    }
}
