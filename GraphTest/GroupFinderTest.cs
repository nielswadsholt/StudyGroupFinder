using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyGroupFinder;
using Graph;
using Util;

namespace StudyGroupFinderTest
{
    [TestClass]
    public class GroupFinderTest
    {
        [TestMethod]
        public void TestPerson()
        {
            Console.WriteLine("\n========== Test: Person ==========\n");
            var person = new Person("Hans");
        }

        [TestMethod]
        public void TestStudent()
        {
            Console.WriteLine("\n========== Test: Student ==========\n");
            var student = new Student("Hans", "Lingvistik");
            Console.WriteLine(student.Name);
            Console.WriteLine(student.Study);
            Console.WriteLine(student.SeeksGroup);
            student.Attributes.ForEach(a => Console.WriteLine(a));
            student.StudyAttributes.ForEach(a => Console.WriteLine(a));
        }

        [TestMethod]
        public void TestStudentGenerator()
        {
            Console.WriteLine("\n========== Test: StudentGenerator ==========\n");
            for (int i = 0; i < 101; i++)
            {
                Student student = StudentGenerator.NextStudent;
                Console.WriteLine(student + ", " + student.Study + ", " + student.SeeksGroup);
                Console.WriteLine(student.Attributes.ToSeparatedString(", "));
                Console.WriteLine(student.StudyAttributes.ToSeparatedString(", "));
            }
        }
    }
}
