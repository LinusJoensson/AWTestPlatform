using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.ViewModels
{
    public class TestSettingsFormVM
    {
        public int? Id { get; set; }

        [Display(Name = "Test Name")]
        [Required(ErrorMessage = "The test must have a name")]
        public string TestName { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "The test must have a description")]
        public string Description { get; set; }

        [Display(Name = "Time Limit")]
        public TimeSpan TimeLimit { get; set; }

        [Display(Name = "Tags")]
        public string Tags { get; set; }
    }
}
