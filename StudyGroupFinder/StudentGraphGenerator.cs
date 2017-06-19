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

        /// <summary>
        /// Returns a hard-coded sample graph containing 20 students with 0-5 neighbors each.
        /// </summary>
        /// <returns></returns>
        public static Digraph<Student> Sample()
        {
            // To reproduce console_test04.txt, run StudyGroupFinderConsole with these arguments:
            //=============================== Ny studerende ================================

            //Indtast navn: Aya
            //Da systemet allerede indeholdt en studerende med dit navn, er dit brugernavn ændret til Aya1
            //Indtast studie: Fransk
            //Ønsker du at deltage i en studiegruppe? (J / N) j
            //Indtast dine personlige egenskaber adskilt af mellemrum: P g r q k
            //Indtast dine studierelevante egenskaber adskilt af mellemrum: ø a s i
            //Indtast navnene på de studerende, der bor tæt på dig, adskilt af mellemrum: Naja Luna Lea

            var digraph = new Digraph<Student>();

            List<Student> students = new List<Student>()
            {
                new Student("Lucas", "Idræt") { SeeksGroup = true, Attributes = new HashSet<string>{ "M", "L", "Y", "Å", "J", "I", "Ø" },  StudyAttributes = new HashSet<string>{ "C" } },
                new Student("Theodor", "IT-Økonomi") { SeeksGroup = true, Attributes = new HashSet<string>{ "E", "K", "Z", "P", "Y", "C", "R", "T" },  StudyAttributes = new HashSet<string>{ "P", "D", "T", "X", "Æ", "O", "R" } },
                new Student("Lea", "Idræt") { SeeksGroup = true, Attributes = new HashSet<string>{ "H" },  StudyAttributes = new HashSet<string>{ "N", "C", "F" } },
                new Student("Filippa", "Matematik") { SeeksGroup = false, Attributes = new HashSet<string>{ "F", "J", "C" },  StudyAttributes = new HashSet<string>{ "C", "S" } },
                new Student("Viggo", "Filosofi") { SeeksGroup = true, Attributes = new HashSet<string>{ "H", "E", "F", "V", "P", "I" },  StudyAttributes = new HashSet<string>{ "R", "A", "K", "B", "Q", "N", "P" } },
                new Student("Bertram", "Psykologi") { SeeksGroup = true, Attributes = new HashSet<string>{ },  StudyAttributes = new HashSet<string>{ "N", "R", "G" } },
                new Student("Luna", "Fransk") { SeeksGroup = true, Attributes = new HashSet<string>{ "P", "N", "X", "R" },  StudyAttributes = new HashSet<string>{ "Y" } },
                new Student("Ida", "IT-Økonomi") { SeeksGroup = true, Attributes = new HashSet<string>{ },  StudyAttributes = new HashSet<string>{ "J", "Å", "O" } },
                new Student("Naja", "Tysk") { SeeksGroup = true, Attributes = new HashSet<string>{ "Q", "Y" },  StudyAttributes = new HashSet<string>{ "H", "Q", "Y", "M" } },
                new Student("Jonathan", "Fransk") { SeeksGroup = false, Attributes = new HashSet<string>{ "A", "U", "Æ", "Å", "Q", "Z" },  StudyAttributes = new HashSet<string>{ "Å", "X" } },
                new Student("Emilie", "IT-Økonomi") { SeeksGroup = false, Attributes = new HashSet<string>{ "O", "G", "N", "M", "I", "F", "Y", "K" },  StudyAttributes = new HashSet<string>{ "F", "H" } },
                new Student("Alberte", "Fransk") { SeeksGroup = true, Attributes = new HashSet<string>{ "B" },  StudyAttributes = new HashSet<string>{ "T", "A", "K", "L", "X", "B", "F" } },
                new Student("Aya", "Datalogi") { SeeksGroup = true, Attributes = new HashSet<string>{ "G" },  StudyAttributes = new HashSet<string>{ "K", "E", "P", "A", "Æ", "B", "C" } },
                new Student("Sofie", "Tysk") { SeeksGroup = true, Attributes = new HashSet<string>{ "L", "K", "U", "P", "R" },  StudyAttributes = new HashSet<string>{ "S", "N", "Y", "J", "I", "X", "A", "M" } },
                new Student("Clara", "Tysk") { SeeksGroup = false, Attributes = new HashSet<string>{ "L", "Å", "M" },  StudyAttributes = new HashSet<string>{ "D", "O", "X", "J", "E" } },
                new Student("Mille", "Matematik") { SeeksGroup = true, Attributes = new HashSet<string>{ "Y", "M" },  StudyAttributes = new HashSet<string>{ "B", "O", "P", "D", "R", "G", "H", "Z" } },
                new Student("Molly", "Idræt") { SeeksGroup = false, Attributes = new HashSet<string>{ "Z" },  StudyAttributes = new HashSet<string>{ "F", "Z", "X", "Å", "E" } },
                new Student("Marius", "Datalogi") { SeeksGroup = true, Attributes = new HashSet<string>{ "G", "V" },  StudyAttributes = new HashSet<string>{ "L", "I", "Y" }  },
                new Student("August", "IT-Økonomi") { SeeksGroup = true, Attributes = new HashSet<string>{ "Q", "T", "L", "S", "U", "H", "K", "G" },  StudyAttributes = new HashSet<string>{ "G", "B", "A" } },
                new Student("Jakob", "Matematik") { SeeksGroup = true, Attributes = new HashSet<string>{ "D", "A", "I", "T", "J", "X", "Y", "V" },  StudyAttributes = new HashSet<string>{ } }
            };

            Dictionary<string, List<string>> adjacencyList = new Dictionary<string, List<string>>()
            {
                { "Lucas", new List<string> { "Molly", "Naja" } },
                { "Theodor", new List<string> { "Jakob", "Luna", "Molly", "Bertram" } },
                { "Lea", new List<string> { "Filippa", "Lucas", "Jakob" } },
                { "Filippa", new List<string> { } },
                { "Viggo", new List<string> { "Marius", "Alberte" } },
                { "Bertram", new List<string> { "Theodor", "Ida", "Aya", "Sofie", "Marius" } },
                { "Luna", new List<string> { "Lea", "Sofie", "Molly" } },
                { "Ida", new List<string> { "August", "Emilie", "Aya", "Alberte" } },
                { "Naja", new List<string> { } },
                { "Jonathan", new List<string> { "Luna" } },
                { "Emilie", new List<string> { "Theodor", "Viggo" } },
                { "Alberte", new List<string> { "Molly", "Luna", "Filippa", "Theodor" } },
                { "Aya", new List<string> { "Theodor" } },
                { "Sofie", new List<string> { "Theodor", "Lea", "Ida" } },
                { "Clara", new List<string> { "Bertram", "Filippa" } },
                { "Mille", new List<string> { "Emilie", "Bertram", "Marius", "Aya" } },
                { "Molly", new List<string> { } },
                { "Marius", new List<string> { "Lucas", "Aya" } },
                { "August", new List<string> { "Aya", "Emilie" } },
                { "Jakob", new List<string> { "Marius", "Theodor", "Ida", "Jonathan" } }
            };

            foreach (Student student in students)
            {
                digraph.AddNode(new Node<Student>(student));
            }

            foreach (string name in adjacencyList.Keys)
            {
                foreach (string neighbor in adjacencyList[name])
                {
                    digraph.AddEdge(name, neighbor);
                }
            }

            return digraph;
        }
    }
}
