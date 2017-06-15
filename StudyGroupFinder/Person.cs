using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyGroupFinder
{
    public class Person
    {
        public string Name { get; private set; }
        public HashSet<string> Attributes { get; set; }

        public Person(string name)
        {
            Name = name;
            Attributes = new HashSet<string>();
        }
    }
}
