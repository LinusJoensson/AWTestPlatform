using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models.Enums;

namespace TestPlatform.ViewModels
{
    public class QuestionFormVM
    {

        public string TextQuestion { get; set; }
        public bool HasComment { get; set; }
        public string Comment { get; set; }
        public string[] SelectedAnswers { get; set; }
        public bool IsInTestSession { get; set; }
        public QuestionType QuestionType { get; set; }

    }
}
