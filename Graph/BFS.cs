using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Graph
{
    /// <summary>
    /// Generic implementation of breadth-first search.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BFS<T> : IPathFinder<T>
    {
        /// <summary>
        /// Returns the shortest path between a given node and the first matching node in the
        /// given graph using breadth-first search. If none exists, the returned path is empty. 
        /// </summary>
        /// <param name="digraph"></param>
        /// <param name="source"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public Path FindPath(Digraph<T> digraph, Node<T> source, Predicate<T> match)
        {
            var path = new Path();
            var parents = new Dictionary<string, string>();
            var frontier = new Queue<string>();
            parents[source.ToString()] = "";
            frontier.Enqueue(source.ToString());

            while (frontier.Count > 0)
            {
                string current = frontier.Dequeue();

                if (match(digraph[current].Data) && current != source.ToString())
                {
                    path.Extend(current);

                    while (parents[current] != "")
                    {
                        path.Extend(parents[current]);
                        current = parents[current];
                    }

                    return path;
                }

                digraph[current].Neighbors.ForEach(n =>
                {
                    string neighbor = n.ToString();
                    if (!parents.ContainsKey(neighbor))
                    {
                        frontier.Enqueue(neighbor);
                        parents[neighbor] = current;
                    }
                });
            }

            // Uncomment to allow source-to-source path:
            //if (match(source.Data)) { path.Extend(source.ToString()); }

            return path;
        }
    }
}
