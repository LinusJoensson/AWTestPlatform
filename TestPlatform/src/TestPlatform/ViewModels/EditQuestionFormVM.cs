using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models.Enums;

namespace TestPlatform.ViewModels
{
    public class EditQuestionFormVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "A Question is required")]
        public string QuestionText { get; set; }

        public bool HasComment { get; set; }

        [Display(Name = "Sort order")]
        [Range(0, int.MaxValue, ErrorMessage = "Must  be a number")]
        [Required(ErrorMessage = "Sort order is required")]
        public int? SortOrder { get; set; }
        
        [Display(Name = "Question type")]
        [Required(ErrorMessage = "Must choose a type.")]
        public QuestionType Type { get; set; }

        public List<AnswerDetailVM> Answers { get; set; }
    }
}
