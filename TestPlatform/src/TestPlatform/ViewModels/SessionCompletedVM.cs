using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Controllers;
using TestPlatform.Models.Enums;

namespace TestPlatform.ViewModels
{
    public class SessionCompletedVM
    {
        public bool? IsSuccessfull { get; set; }
        public SessionCompletedReason SessionCompletedReason { get; set; }

    }
}
