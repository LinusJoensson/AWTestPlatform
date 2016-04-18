using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public TestCategory Category { get; set; }
        public List<string> Tags { get; set; }
        public bool IsPublished { get; set; }
        public virtual List<Question> Questions { get; set; }
    }
}
