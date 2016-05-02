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
        ManageTestQuestionsVM GetManageTestQuestionVM(int testId);
        void RemoveQuestionFromTest(int questionId, int testId);
        int CreateAnswer(int questionId);
        int CreateAnswer(int questionId, AnswerDetailVM viewModel);
        int CreateTestFromTemplate(int testId);
        QuestionFormVM GetPreviewQuestion(int questionId);
        int CreateTestQuestion(int testId);
        Answer[] GetAllAnswers();
        void RemoveAnswerFromQuestion(int testId, int questionId, int answerId);
    }
}
