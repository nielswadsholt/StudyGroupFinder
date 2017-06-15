using Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyGroupFinder
{
    public static class StudentGraphGenerator
    {
        /// <summary>
        /// Returns a directed graph containing randomly generated students,
        /// each with a random number of neighbors.
        /// </summary>
        /// <param name="studentCount"></param>
        /// <param name="maxNeighborCount"></param>
        /// <returns></returns>
        public static Digraph<Student> Generate(int studentCount, int maxNeighborCount)
        {
            var digraph = new Digraph<Student>();

            for (int i = 0; i < studentCount; i++)
            {
                Student student = StudentGenerator.NextStudent;
                digraph.AddNode(new Node<Student>(student));
            }

            string[] names = (digraph.Nodes.Keys).ToArray();
            Random rnd = new Random();

            foreach (string name in names)
            {
                int numNeighbors = rnd.Next(1 + maxNeighborCount);
                for (int i = 0; i < numNeighbors; i++)
                {
                    digraph.AddEdge(name, names[rnd.Next(names.Length)]);
                }
            }

            return digraph;
        }
    }
}
