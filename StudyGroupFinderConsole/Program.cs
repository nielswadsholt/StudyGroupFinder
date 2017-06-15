using Graph;
using StudyGroupFinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace StudyGroupFinderConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // BUG: 42 is missing :-(

            Console.Title = "Studiegruppefinderen v. 1.0";
            Console.WriteLine("******************************************************************************");
            Console.WriteLine("*                  S T U D I E G R U P P E F I N D E R E N                   *");
            Console.WriteLine("*                                  v. 1.0                                    *");
            Console.WriteLine("******************************************************************************");

            // Generate student graph
            var digraph = StudentGraphGenerator.Generate(20, 5);

            Console.WriteLine("\n=========================== Registrerede studerende ===========================\n");
            Console.WriteLine("(Navn,  studie,  søger studiegruppe?,  [egenskaber],  {studierelevante egenskaber})");
            foreach (Node<Student> node in digraph.Nodes.Values)
            {
                Console.Write(node.Data + ",  " + node.Data.Study + ",  " + (node.Data.SeeksGroup ? "Ja" : "Nej") + ",  ");
                Console.Write("[" + node.Data.Attributes.ToSeparatedString(", ") + "],  ");
                Console.WriteLine("{" + node.Data.StudyAttributes.ToSeparatedString(", ") + "}");
            }

            // TODO: Input validation
            // Strengene må ikke være tomme.
            string studentName = "";
            List<string> neighbors = new List<string>();
            Node<Student> newNode = new Node<Student>();
            bool approved = false;

            while (!approved)
            {
                Console.WriteLine("\n=============================== Ny studerende ================================\n");

                studentName = "";
                while (studentName == "")
                {
                    Console.Write("Indtast navn: ");
                    studentName = Console.ReadLine();
                }

                if (digraph.Contains(studentName))
                {
                    string newName = studentName;
                    int suffix = 1;

                    while (digraph.Contains(newName))
                    {
                        newName = studentName + suffix;
                        suffix += 1;
                    }

                    studentName = newName;

                    Console.WriteLine("Da systemet allerede indeholdt en studerende med dit navn, er dit brugernavn ændret til " + studentName);
                }

                string study = "";
                while (study == "")
                {
                    Console.Write("Indtast studie: ");
                    study = Console.ReadLine();
                }

                string seeksGroup = "";

                while (!new string[] { "J", "N" }.Contains(seeksGroup.ToUpper()))
                {
                    Console.Write("Ønsker du at deltage i en studiegruppe? (J/N) ");
                    seeksGroup = Console.ReadLine();
                }

                Console.Write("Indtast dine personlige egenskaber adskilt af mellemrum: ");
                HashSet<string> attributes = new HashSet<string>(Console.ReadLine().ToUpper().Split(' '));
                Console.Write("Indtast dine studierelevante egenskaber adskilt af mellemrum: ");
                HashSet<string> studyAttributes = new HashSet<string>(Console.ReadLine().ToUpper().Split(' '));

                bool validNeighbors = true;

                do
                {
                    Console.Write("Indtast navnene på de studerende, der bor tæt på dig, adskilt af mellemrum: ");
                    string neighborsString = Console.ReadLine();
                    neighbors = neighborsString == "" ? new List<string>() : neighborsString.Split(' ').ToList();
                    validNeighbors = true;

                    foreach (string neighbor in neighbors)
                    {
                        if (!digraph.Nodes.ContainsKey(neighbor))
                        {
                            validNeighbors = false;
                            Console.WriteLine(neighbor + " blev ikke fundet i systemet. Prøv venligst igen. ");
                            break;
                        }
                    }
                }
                while (!validNeighbors);

                Student newStudent = new Student(studentName, study);
                newStudent.Attributes = attributes;
                newStudent.StudyAttributes = studyAttributes;

                if (seeksGroup.ToUpper() == "J")
                {
                    newStudent.SeeksGroup = true;
                }
                else
                {
                    newStudent.SeeksGroup = false;
                }

                newNode = new Node<Student>(newStudent);

                Console.WriteLine("\n============================== Dine oplysninger ==============================\n");
                Console.WriteLine(newStudent + ", " + newStudent.Study + ", " + (newStudent.SeeksGroup ? "søger studiegruppe" : "søger ikke studiegruppe"));
                Console.WriteLine("Personlige egenskaber: " + newStudent.Attributes.ToSeparatedString(", "));
                Console.WriteLine("Studierelevante egenskaber: " + newStudent.StudyAttributes.ToSeparatedString(", "));
                Console.WriteLine("Dine naboer: " + neighbors.ToSeparatedString(", "));
                Console.WriteLine();

                string appr = "";
                while (!new string[] { "J", "N" }.Contains(appr))
                {
                    Console.Write("Kan oplysningerne godkendes? (J/N) ");
                    appr = Console.ReadLine().ToUpper();
                }

                approved = appr == "J";
            }

            neighbors.ForEach(n => newNode.AddNeighbor(digraph[n]));
            digraph.AddNode(newNode);

            Console.WriteLine("\n================================== RESULTAT ==================================\n");
                        
            Console.WriteLine("Nærmeste med samme fag:");
            Console.WriteLine(digraph.FindPath(newNode, n => newNode.Data.Study == n.Study));

            if (newNode.Data.SeeksGroup)
            {
                Console.WriteLine("\nNærmeste med samme fag, som søger studiegruppe:");
                Console.WriteLine(digraph.FindPath(newNode, n => n.SeeksGroup && newNode.Data.Study == n.Study));

                Console.WriteLine("\nNærmeste med samme fag, som søger studiegruppe + har mindst én studierelevant egenskab tilfælles:");
                Console.WriteLine(digraph.FindPath(newNode, n =>
                    n.SeeksGroup &&
                    newNode.Data.Study == n.Study &&
                    (newNode.Data.StudyAttributes.Intersect(n.StudyAttributes)).Count() > 0));
            }

            Console.WriteLine("\nNærmeste med mindst én egenskab tilfælles:");
            Console.WriteLine(digraph.FindPath(newNode, n => (newNode.Data.Attributes.Intersect(n.Attributes)).Count() > 0));

            Console.WriteLine("\nNærmeste med mindst tre egenskaber tilfælles:");
            Console.WriteLine(digraph.FindPath(newNode, n => (newNode.Data.Attributes.Intersect(n.Attributes)).Count() > 2));

            Console.WriteLine("\nNærmeste med egenskaben 'A':");
            Console.WriteLine(digraph.FindPath(newNode, n => n.Attributes.Contains("A")));

            Console.WriteLine("\nKorteste rute til hver af de andre studerende (BFS):");
            foreach (KeyValuePair<string, string> dest in digraph.FindPathsToAll(studentName))
            {
                Console.WriteLine($"{studentName} til {dest.Key}:  " + dest.Value); ;
            }

            digraph.PathFinder = new DFS<Student>();
            Console.WriteLine("\nVilkårlig rute til hver af de andre studerende (DFS):");
            foreach (KeyValuePair<string, string> dest in digraph.FindPathsToAll(studentName))
            {
                Console.WriteLine($"{studentName} til {dest.Key}:  " + dest.Value); ;
            }

            Console.WriteLine("\nOversigt over naboskaber:");
            Console.WriteLine(digraph);

            Console.WriteLine("\nTryk på en vilkårlig tast for at afslutte ...");
            Console.ReadKey();
        }
    }
}
