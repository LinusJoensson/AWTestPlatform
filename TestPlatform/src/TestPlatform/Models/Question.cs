using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models.Enums;

namespace TestPlatform.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int? TestId { get; set; } //NOTE: allowed null
        public string Name { get; set; }
        public string TextQuestion { get; set; }
        public int SortOrder { get; set; }
        public QuestionType QuestionType { get; set; }

        public string Author { get; set; }
        public string Category { get; set; }
        public List<string> ArbitraryTags { get; set; }

        public virtual List<Answer> Answers { get; set; }
    }
}
