using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.Models
{
    public class TestSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? SubmitTime { get; set; }
        public List<QuestionResult> QuestionResults { get; set; }

        public TestSession()
        {
            QuestionResults = new List<QuestionResult>();
        }

    }
}
