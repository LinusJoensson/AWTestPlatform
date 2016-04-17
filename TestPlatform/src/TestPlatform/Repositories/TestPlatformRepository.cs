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

        public TestPlatformRepository()
        {
            _tests = new List<Test>();
            _users = new List<User>();
            _questions = new List<Question>();
            
            _questions.Add(new Question()
            {
                Id = 1,
                Name = "First Question",
                TextQuestion = "Is this the first question?",
                QuestionType = QuestionType.SingleChoice,
                SortOrder = 0,
                ArbitraryTags = new List<string>() { "for lolz", "eazy" },
                Author = "Linus Joensson",
                Category = "Random",
                Answers = new List<Answer>()
                {
                    new Answer() { Id = 1, QuestionId = 1,  IsCorrect = true, TextAnswer = "Yes" },
                    new Answer() { Id = 2, QuestionId = 1,  IsCorrect = false, TextAnswer = "Yes! ... Uhm I mean no" }
                }
            });

            _tests.Add(new Test()
            {
                Id = 1,
                ArbitraryTags = new List<string>() { "Eazy", "awesome", "heavy" },
                Author = "Linus Joensson",
                Category = "Rabdom",
                Name = "My First Test",
                Description = "An eazy test"
            });

            var selectedQuestion = _questions.Find(o => o.Id == 1);
            _tests.Last().Questions.Add(new Question()
            {
                Id = 2,
                TestId = _tests.Last().Id,
                Answers = selectedQuestion.Answers,
                ArbitraryTags = selectedQuestion.ArbitraryTags,
                Author = selectedQuestion.Author,
                Category = selectedQuestion.Category,
                Name = selectedQuestion.Name,
                QuestionType = selectedQuestion.QuestionType,
                SortOrder = 100,
                TextQuestion = selectedQuestion.TextQuestion
            });
        }
    }
}
