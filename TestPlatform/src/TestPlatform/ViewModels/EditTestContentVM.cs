using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.ViewModels.GridViewModels;

namespace TestPlatform.ViewModels
{
    public class EditTestContentVM
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public GridQuestionsVM GridAllQuestions { get; set; }
        public GridQuestionsVM GridTestQuestions { get; set; }
    }
}
