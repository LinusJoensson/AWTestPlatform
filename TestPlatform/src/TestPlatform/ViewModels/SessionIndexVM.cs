using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.ViewModels
{
    public class SessionIndexVM
    {
        public int TestId { get; set; }
        public int UserId { get; set; }
        public string TestName { get; set; }
        public string TestDescription { get; set; }
        public int NumberOfQuestions { get; set; }
    }
}
