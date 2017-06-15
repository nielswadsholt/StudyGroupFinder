using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyGroupFinder
{
    /// <summary>
    /// Represents a student entity.
    /// </summary>
    public class Student : Person
    {
        public string Study { get; private set; }
        public HashSet<string> StudyAttributes { get; set; }
        public bool SeeksGroup { get; set; }

        public Student(string name, string study) : base(name)
        {
            Study = study;
            StudyAttributes = new HashSet<string>();
        }

        /// <summary>
        /// Returns a string representation of the student.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
