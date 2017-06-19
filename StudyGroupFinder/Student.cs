using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace StudyGroupFinder
{
    /// <summary>
    /// Represents a student entity.
    /// </summary>
    public class Student : Person
    {
        public string Study { get; set; }
        public HashSet<string> StudyAttributes { get; set; }
        public bool SeeksGroup { get; set; }

        public Student(string name) : base(name)
        {
            Study = "";
            StudyAttributes = new HashSet<string>();
        }

        public Student(string name, string study) : base(name)
        {
            Study = study;
            StudyAttributes = new HashSet<string>();
        }

        public string GetDetails()
        {
            StringBuilder sb = new StringBuilder($"Name: { Name }");
            sb.Append($", Study: { Study }");
            sb.Append($", Seeks group: { SeeksGroup }");
            sb.Append($", Attributes: [{ Attributes.ToSeparatedString() }]");
            sb.Append($", Study attributes: [{ StudyAttributes.ToSeparatedString() }]");
            return sb.ToString();
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
