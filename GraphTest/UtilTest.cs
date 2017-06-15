using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using StudyGroupFinder;
using Util;

namespace StudyGroupFinderTest
{
    [TestClass]
    public class UtilTest
    {
        [TestMethod]
        public void TestExtensions()
        {
            Console.WriteLine("\n========== Test: Extensions ==========");

            // ForEach()
            Console.WriteLine("\n========== IEnumarable<T>.ForEach() ==========");
            HashSet<Student> students = null;
            students.ForEach(s => Console.WriteLine(s.Name));

            // Items can be null without throwing any exception as long as they don't
            // violate the given Action<T>:
            var nullableInts = new HashSet<int?> { 0, null, 2, 3, null };
            nullableInts.ForEach(ni => Console.Write($"{ni} "));
            Console.WriteLine();

            // If they do, however, a NullReferenceException is thrown (as expected):
            students = new HashSet<Student> { new Student("Hans", "Kemi"), null };
            try
            {
                students.ForEach(s => Console.WriteLine(s.Name));
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex);
            }

            // ToSeparatedString()
            Console.WriteLine("\n========== IEnumarable<T>.ToSeparatedString() ==========");
            Dictionary<int, string> dict = new Dictionary<int, string>
            {
                { 0, "zero" },
                { 1, "one" },
                { 2, "two" },
                { 3, null }
            };
            Console.WriteLine(dict.ToSeparatedString());
            Console.WriteLine(dict.ToSeparatedString(", "));
            Console.WriteLine(dict.ToSeparatedString(" -> "));

            HashSet<object> empty = new HashSet<object>();
            Console.WriteLine(empty.ToSeparatedString(" "));
            Console.WriteLine("(No NullReferenceException thrown)");
        }
    }
}
