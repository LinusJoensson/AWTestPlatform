using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.ViewModels.GridViewModels
{
    public class AnswerDetailVM
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool ShowAsCorrect { get; set; }
        public bool IsChecked { get; set; }
    }
}
