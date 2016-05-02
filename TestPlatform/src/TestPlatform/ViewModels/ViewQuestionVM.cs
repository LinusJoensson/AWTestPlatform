using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.ViewModels
{
    public class ViewQuestionVM
    {
        public string TestTitle { get; set; }
        public int QuestionIndex { get; set; }
        public int NumOfQuestion { get; set; }
        public int SecondsLeft { get; set; }
        public QuestionFormVM QuestionFormVM { get; set; }
    }
}
