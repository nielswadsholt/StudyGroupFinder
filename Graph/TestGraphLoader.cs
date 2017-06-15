using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Graph
{
    public static class TestGraphLoader
    {
        public static Digraph<int> LoadGraph(string path)
        {
            var digraph = new Digraph<int>();

            using (var file = new System.IO.StreamReader(path))
            {
                // Number of nodes
                int size = int.Parse(file.ReadLine());

                // Add nodes
                for (int i = 0; i < size; i++)
                {
                    digraph.AddNode(new Node<int>(i));
                }

                // Number of edges
                file.ReadLine();
                string line;

                // Add edges
                while ((line = file.ReadLine()) != null)
                {
                    var matches = Regex.Matches(line, @"\d+");
                    digraph.AddEdge(matches[0].Value, matches[1].Value);
                }
            }

            return digraph;
        }
    }
}
