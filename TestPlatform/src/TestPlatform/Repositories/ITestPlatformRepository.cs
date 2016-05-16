using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Controllers;
using TestPlatform.Models;
using TestPlatform.Models.Enums;
using TestPlatform.ViewModels;

namespace TestPlatform.Repositories
{
    public interface ITestPlatformRepository
    {
        int CreateTest(Test test);
        Test[] GetAllTests();
        Question[] GetAllQuestions();
        void CopyQuestionToTest(int questionId, int testId);
        ViewQuestionVM GetViewQuestion(int testSessionId, int questionIndex, bool isInSession);
        bool UpdateSessionAnswers(int testSessionId, int questionIndex, string[] selectedAnswers, string comment);
        SessionIndexVM GetSessionIndexVM(int testId);
        int StartNewSession(int userId, int testId);
        void SubmitTestSession(int testSessionId);
        TestSession GetTestSessionById(int testSessionId);
        ManageTestQuestionsVM GetManageTestQuestionVM(int testId);
        void RemoveQuestionFromTest(int questionId, int testId);
        ShowResultsVM GetShowResultsVM(int testId);
        int CreateAnswer(int questionId);
        int CreateAnswer(int questionId, AnswerDetailVM viewModel);
        QuestionFormVM GetPreviewQuestion(int questionId);
        int CreateTestQuestion(int testId);
        Answer[] GetAllAnswers();
        SessionCompletedVM GetSessionCompletedVM(int testSessionId, SessionCompletedReason sessionCompletedReason);
        void RemoveAnswerFromQuestion(int testId, int questionId, int answerId);
        EditQuestionVM GetEditQuestionVM(int testId, int questionId);
        QuestionFormVM GetPreviewQuestionPartial(int questionId);
    }
}
