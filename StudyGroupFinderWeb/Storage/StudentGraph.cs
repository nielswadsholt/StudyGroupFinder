using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Graph;
using StudyGroupFinder;

namespace StudyGroupFinderWeb.Storage
{
    public static class StudentGraph
    {
        public static Digraph<Student> Students { get; set; }

        static StudentGraph()
        {
            Students = StudentGraphGenerator.Generate(100, 5);
        }

        public static void Load()
        {
            
        }

        public static void Add(Student student)
        {
            Students.AddNode(new Node<Student>(student));
        }

        public static void Add(Node<Student> node)
        {
            Students.AddNode(node);
        }

        public static Node<Student> Get(string name)
        {
            return Students[name];
        }

        public static List<Node<Student>> GetAll()
        {
            return Students.Nodes.Values.ToList();
        }

        public static List<Node<Student>> GetAllBut(List<Node<Student>> subtract)
        {
            return Students.Nodes.Values.Except(subtract).ToList();
        }

        //TODO: Remove(string name)
    }
}