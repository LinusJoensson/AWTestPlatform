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
        [Range(1, 1440, ErrorMessage = "Number must be between 1 and 1440")]
        public int? TimeLimitInMinutes { get; set; }

        [Display(Name = "Tags")]
        public string Tags { get; set; }

        [Display(Name = "Show pass or fail")]
        public bool ShowPassOrFail { get; set; }

        [Display(Name = "Show test score")]
        public bool ShowTestScore { get; set; }

        [Display(Name = "Pass threshhold (%)")]
        public int? PassPercentage { get; set; }

        [Display(Name = "Custom message")]
        public string CustomCompletionMessage { get; set; }

        [Display(Name = "Name of certificate PDF template (*.pdf)")]
        public string CertTemplatePath { get; set; }

        [Display(Name = "Enable certificate download")]
        public bool EnableCertDownloadOnCompletion { get; set; }

        [Display(Name = "Enable certificate by email")]
        public bool EnableEmailCertOnCompletion { get; set; }
    }
}
