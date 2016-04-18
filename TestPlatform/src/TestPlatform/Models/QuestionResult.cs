﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.Models
{
    public class QuestionResult
    {
        public int Id { get; set; }
        public int QuestionID { get; set; }
        public string Comment { get; set; }
        public string SelectedAnswers { get; set; }
    }
}
