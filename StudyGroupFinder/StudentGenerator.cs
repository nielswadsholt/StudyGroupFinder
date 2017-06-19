using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;
using Util;

namespace StudyGroupFinder
{
    /// <summary>
    /// Provides a single static method for generating random students with unique names.
    /// Implemented using the singleton design pattern.
    /// </summary>
    public sealed class StudentGenerator
    {
        private static int batchNo = 1;
        private static readonly StudentGenerator instance = new StudentGenerator();

        /// <summary>
        /// Returns the StudentGenerator instance.
        /// </summary>
        public static StudentGenerator Instance
        {
            get { return instance; }
        }

        private Queue<Student> students;

        private StudentGenerator()
        {
            students = new Queue<Student>();
            Load();
        }

        /// <summary>
        /// Loads the students queue with an optional suffix added to each name.
        /// </summary>
        private void Load(string suffix = "")
        {
            // NOTE: The main directory path is absolute to accomodate StudyGroupFinderWeb
            // Must be changed before running on a different machine! 
            string mainDirectory = @"";
            List<string> names = Helpers.LoadStrings(mainDirectory + @"StudyGroupFinder/Data/names.txt");
            List<string> studies = Helpers.LoadStrings(mainDirectory + @"StudyGroupFinder/Data/studies.txt");
            List<string> attributes = Helpers.LoadStrings(mainDirectory + @"StudyGroupFinder/Data/attributes.txt");
            List<string> study_attributes = Helpers.LoadStrings(mainDirectory + @"StudyGroupFinder/Data/study_attributes.txt");
            names.Shuffle();
            var rnd = new Random();

            foreach (string name in names)
            {
                var student = new Student(name + suffix, studies[rnd.Next(0, studies.Count)]);

                int numAttr = rnd.Next(0, 10);
                for (int i = 0; i < numAttr; i++)
                {
                    student.Attributes.Add(attributes[rnd.Next(0, attributes.Count)]);
                }

                numAttr = rnd.Next(0, 10);
                for (int i = 0; i < numAttr; i++)
                {
                    student.StudyAttributes.Add(study_attributes[rnd.Next(0, study_attributes.Count)]);
                }

                // Assertion: Most students seek a study group
                student.SeeksGroup = rnd.Next(0, 7) > 0;

                students.Enqueue(student);
            }
        }

        /// <summary>
        /// Reloads the students queue with an incremented number suffix added
        /// to each name to ensure uniqueness.
        /// </summary>
        private void Reload()
        {
            Load(batchNo++.ToString());
        }

        /// <summary>
        /// Returns a randomly generated student with a unique name.
        /// </summary>
        /// <returns></returns>
        public Student Next()
        {
            if (students.Count < 1) { Reload(); }
            return students.Dequeue();
        }

        /// <summary>
        /// Returns a randomly generated student with a unique name.
        /// </summary>
        /// <returns></returns>
        public static Student NextStudent => Instance.Next();
    }
}
