using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.ViewModels.GridViewModels;

namespace TestPlatform.ViewModels
{
    public class EditTestContentVM
    {
        public GridQuestionsVM GridAllQuestionsVM { get; set; }
        public GridQuestionsVM GridTestQuestionsVM { get; set; }
    }
}
