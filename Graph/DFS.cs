using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Graph
{
    /// <summary>
    /// Generic implementation of depth-first search.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DFS<T> : IPathFinder<T>
    {
        /// <summary>
        /// Returns a path between a source node and the first matching node in the
        /// graph using depth-first search. If none exists, the returned path is empty.
        /// The returned path is not guaranteed to be the shortest one.
        /// </summary>
        /// <param name="digraph"></param>
        /// <param name="source"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public Path FindPath(Digraph<T> digraph, Node<T> source, Predicate<T> match)
        {
            var parents = new Dictionary<string, string>();
            parents[source.ToString()] = "";
            string result = RecursiveDFS(digraph, source.ToString(), match, parents);
            Path path = new Path();

            if (result != "")
            {
                path.Extend(result);

                while (parents[result] != "")
                {
                    path.Extend(parents[result]);
                    result = parents[result];
                }
            }

            // Uncomment to allow source-to-source path:
            //if (match(source.Data)) { path.Extend(source.ToString()); }

            return path;
        }

        /// <summary>
        /// Recursive helper method for FindPath that searches the graph in depth-first order.
        /// </summary>
        /// <param name="digraph"></param>
        /// <param name="current"></param>
        /// <param name="match"></param>
        /// <param name="parents"></param>
        /// <returns></returns>
        private string RecursiveDFS(Digraph<T> digraph, string current, Predicate<T> match, Dictionary<string, string> parents)
        {
            if (match(digraph[current].Data) && parents[current] != "")
            {
                return current;
            }

            foreach (Node<T> n in digraph[current].Neighbors)
            {
                string neighbor = n.ToString();

                if (!parents.ContainsKey(neighbor))
                {
                    parents[neighbor] = current;
                    string result = RecursiveDFS(digraph, neighbor, match, parents);

                    if (result != "") { return result; }
                }
            }

            return "";
        }

        public override string ToString()
        {
            return "DFS";
        }
    }
}
