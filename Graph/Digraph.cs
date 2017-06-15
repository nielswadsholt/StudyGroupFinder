using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Graph
{
    /// <summary>
    /// Represents a directed graph consisting of nodes with directed edges between them.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Digraph<T>
    {
        public Dictionary<string, Node<T>> Nodes { get; private set; }
        private IPathFinder<T> pathFinder;

        /// <summary>
        /// Gets or sets the search algorithm used by the graph.
        /// </summary>
        public IPathFinder<T> PathFinder
        {
            get
            {
                return pathFinder;
            }
            set
            {
                if (value is null)
                {
                    throw new ArgumentException("A search algorithm cannot be null.");
                }

                pathFinder = value;
            }
        }


        public Digraph()
        {
            Nodes = new Dictionary<string, Node<T>>();
            PathFinder = new BFS<T>();
        }

        /// <summary>
        /// Read-only indexer that maps directly to Digraph.Nodes' indexer.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Node<T> this[string key] => Nodes[key];

        /// <summary>
        /// Determines whether the graph contains the given node.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return Nodes.ContainsKey(key);
        }

        /// <summary>
        /// Adds the given node to the graph.
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(Node<T> node)
        {
            Nodes.Add(node.ToString(), node);
        }

        /// <summary>
        /// Removes the node with the given name from the graph.
        /// No exception is thrown if the node is not found.
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(string node)
        {
            if (Nodes.ContainsKey(node))
            {
                Nodes.Values.ForEach(n => n.RemoveNeighbor(Nodes[node]));
                Nodes.Remove(node);
            }
        }

        /// <summary>
        /// Adds an edge between two given nodes.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void AddEdge(string from, string to)
        {
            try
            {
                if (from != to)
                {
                    Nodes[from].AddNeighbor(Nodes[to]);
                }                
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("The graph does not contain a node with the given name", ex);
            }
        }

        /// <summary>
        /// Removes any existing edge between the given nodes.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void RemoveEdge(string from, string to)
        {
            try
            {
                Nodes[from].RemoveNeighbor(Nodes[to]);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"One or both of the given nodes do not exist in the graph.", ex);
            }
        }

        /// <summary>
        /// Returns a path between a given node and the first matching node.
        /// If none exists, the returned path is empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public Path FindPath(Node<T> source, Predicate<T> match)
        {
            return PathFinder.FindPath(this, source, match);
        }

        /// <summary>
        /// Returns a path between a given node and the first matching node.
        /// If none exists, the returned path is empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public Path FindPath(string source, Predicate<T> match)
        {
            return FindPath(Nodes[source], match);
        }

        /// <summary>
        /// Returns a path between two given nodes. If none exists, the returned path is empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public Path FindPath(Node<T> source, Node<T> destination)
        {
            return FindPath(source, p => p.Equals(destination.Data));
        }

        /// <summary>
        /// Returns a path between two given nodes. If none exists, the returned path is empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public Path FindPath(string source, string destination)
        {
            return FindPath(Nodes[source], p => p.Equals(Nodes[destination].Data));
        }

        /// <summary>
        /// Returns the shortest path between a given node and the first matching node using
        /// breadth-first search. If none exists, the returned path is empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public Path FindShortestPath(Node<T> source, Predicate<T> match)
        {
            return new BFS<T>().FindPath(this, source, match);
        }
        
        /// <summary>
        /// Returns the shortest path between a given node and the first matching node using
        /// breath-first search. If none exists, the returned path is empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public Path FindShortestPath(string source, Predicate<T> match)
        {
            return FindShortestPath(Nodes[source], match);
        }

        /// <summary>
        /// Returns the shortest path between two given nodes using breadth-first search.
        /// If none exists, the returned path is empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public Path FindShortestPath(string source, string destination)
        {
            return FindShortestPath(Nodes[source], p => p.Equals(Nodes[destination].Data));
        }

        /// <summary>
        /// Returns an iterator over paths between a given node and each of the nodes in the graph.
        /// If a path between two nodes is not found, the returned path is empty.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, string>> FindPathsToAll(string source)
        {
            foreach (string dest in Nodes.Keys)
            {
                yield return new KeyValuePair<string, string>(dest, FindPath(source, dest).ToString());
            }
        }

        /// <summary>
        /// Returns a string representation of the graph in the form of an adjacency list.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            Nodes.Values.ForEach(n =>
            {
                sb.Append($"{n.ToString()}: ");
                sb.Append(n.Neighbors.ToSeparatedString(", "));
                sb.AppendLine();
            });

            return sb.ToString();
        }
    }
}
