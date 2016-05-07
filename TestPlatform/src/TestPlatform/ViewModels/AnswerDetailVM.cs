using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.ViewModels
{
    public class AnswerDetailVM
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool ShowAsCorrect { get; set; }
        public bool IsChecked { get; set; }
        public bool IsInPreview { get; set; }
    }
}
