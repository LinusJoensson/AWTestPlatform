using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models.Enums;

namespace TestPlatform.ViewModels
{
    public class EditQuestionVM
    {
        public bool IsInEdit { get; set; }
        public int QuestionId { get; set; }
        public int TestId { get; set; }
        public int SortOrder { get; set; }
        public bool HasComment { get; set; }
        //Dropdown menu item types
        public SelectListItem[] ItemType { get; set; }

        public QuestionFormVM QuestionFormVM { get; set; }
        public List<AnswerDetailVM> AnswerDetailVMs { get; set; }
        public QuestionType Type { get; set; }
    }
}
