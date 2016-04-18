using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.Models
{
    public class TestCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public List<string> Tags { get; set; }
        public string Author { get; set; }
    }
}
