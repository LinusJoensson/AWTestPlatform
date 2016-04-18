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
    }
}
