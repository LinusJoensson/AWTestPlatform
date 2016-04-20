using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;
using TestPlatform.ViewModels;

namespace TestPlatform.Repositories
{
    public interface ITestPlatformRepository
    {
        int CreateTest(Test test);
        Test[] GetAllTests();
        Question[] GetAllQuestions();
        void AddQuestionToTest(int questionId, int testId);
        ViewQuestionVM GetViewQuestion(int testSessionId, int questionIndex, bool isInSession);
        void UpdateSessionAnswers(int testSessionId, int questionIndex, string[] selectedAnswers, string comment);
        SessionIndexVM GetSessionIndexVM(int testId);
        int StartNewSession(int userId, int testId);
        void SubmitTestSession(int testSessionId);
        TestSession GetTestSessionById(int testSessionId);
        EditTestContentVM GetEditTestContentVM(int id);
        void RemoveQuestionFromTest(int questionId, int testId);
    }
}
