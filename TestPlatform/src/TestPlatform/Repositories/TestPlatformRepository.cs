using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;
using TestPlatform.Models.Enums;

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

        public TestPlatformRepository()
        {
            _tests = new List<Test>();
            _users = new List<User>();
            _questions = new List<Question>();
            _questionCategories = new List<QuestionCategory>();
            _testCategories = new List<TestCategory>();
            _answers = new List<Answer>();

            #region Add static user
            _users.Add(new User()
            {
                Id = 1,
                Email = "linus.joensson.ms@outlook.com",
                FirstName = "Linus",
                Lastname = "Joensson",
                TestSessions = new List<TestSession>()
            });
            #endregion

            #region Add static categories 
            _questionCategories.Add(new QuestionCategory()
            {
                Id = 1,
                Name = "My First Question Category",
                Tags = new List<string>() { "first", "good questions" },
                Author = "Linus Joensson",
                Description = "Very good questions",
            });
            #endregion

            #region Add static questions 
            _questions.Add(new Question()
            {
                Id = 1,
                Name = "First Question",
                TextQuestion = "Is this the first question?",
                QuestionType = QuestionType.SingleChoice,
                Tags = new List<string>() { "for lolz", "eazy" },
                Author = "Linus Joensson",
                Category = _questionCategories.First(),
                Answers = new List<Answer>()
                {
                    new Answer() { Id = _answers.Count() + 1, QuestionId = 1,  IsCorrect = true, TextAnswer = "Yes" },
                    new Answer() { Id = _answers.Count() + 1, QuestionId = 1,  IsCorrect = false, TextAnswer = "Yes! ... Uhm I mean no" }
                }
            });

            _questions.Add(new Question()
            {
                Id = 2,
                Name = "Second Question",
                TextQuestion = "Is this the third question?",
                QuestionType = QuestionType.SingleChoice,
                Tags = new List<string>() { "for lolz", "eazy" },
                Author = "Linus Joensson",
                Category = _questionCategories.First(),
                Answers = new List<Answer>()
                {
                    new Answer() { Id = _answers.Count() + 1, QuestionId = 2,  IsCorrect = false, TextAnswer = "Yes" },
                    new Answer() { Id = _answers.Count() + 1, QuestionId = 2,  IsCorrect = true, TextAnswer = "Yes! ... Uhm I mean no" }
                }
            });

            _questions.Add(new Question()
            {
                Id = 3,
                Name = "Second Question",
                TextQuestion = "Is THIS the third question?",
                QuestionType = QuestionType.SingleChoice,
                Tags = new List<string>() { "for lolz", "hard" },
                Author = "Linus Joensson",
                Category = _questionCategories.First(),
                Answers = new List<Answer>()
                {
                    new Answer() { Id = _answers.Count() + 1, QuestionId = 3,  IsCorrect = true, TextAnswer = "Yes" },
                    new Answer() { Id = _answers.Count() + 1, QuestionId = 3,  IsCorrect = false, TextAnswer = "Yes! ... Uhm I mean no" }
                }
            });

            #endregion

            #region Add static tests
            _tests.Add(new Test()
            {
                Id = 1,
                Tags = new List<string>() { "Eazy", "awesome", "heavy" },
                Author = "Linus Joensson",
                Category = "Random",
                Name = "My First Test",
                Description = "An eazy test",
                Questions = new List<Question>()
            });
            #endregion

            //  Select and add questions just as would be done in action when a test is created from scratch:
            //
            // 1) User selects a question to add to the test
            // 2) The question is collected from the questions table in the database
            // 3) The collected question is added to the test as a duplicate object
            // *  The Duplicated question belongs to the test and may now have test-specific attributes such as SortOrder
            // *  The Original question is left untouched in the questions table 

            var selectedQuestionId = 1;
            var selectedQuestion = _questions.Find(o => o.Id == selectedQuestionId);
            var selectedTest = _tests.ElementAt(0);

            if (selectedQuestion == null)
                throw new KeyNotFoundException("The question id " + selectedQuestionId + " does not exist in database");

            //Note that C# does not support object duplication so we need to do this manually
            selectedTest.Questions.Add(new Question()
            {
                //Test specific attributes
                Id = _questions.Count() + 1,
                TestId = _tests.Last().Id,
                SortOrder = 100,

                //Duplication
                Answers = selectedQuestion.Answers,
                Tags = selectedQuestion.Tags,
                Author = selectedQuestion.Author,
                Category = selectedQuestion.Category,
                Name = selectedQuestion.Name,
                QuestionType = selectedQuestion.QuestionType,
                TextQuestion = selectedQuestion.TextQuestion
            });

            //We use the same database table for questions owned by tests as for questions without owner
            _questions.Add(selectedTest.Questions.Last());
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
                Tags = new List<string>() { "happy", "insane" },
                Author = _users.ElementAt(0).FirstName,
                Category = _testCategories.ElementAt(0),
            });

            return _tests.Last().Id;
        }
    }
}
