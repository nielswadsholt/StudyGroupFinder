using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    /// <summary>
    /// Represents a node in a directed graph. Maintains the node's data
    /// and a set of neighbor nodes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Node<T>
    {
        public T Data { get; private set; }
        public HashSet<Node<T>> Neighbors { get; private set; }

        public Node()
        {
            Neighbors = new HashSet<Node<T>>();
        }

        public Node(T data) : this()
        {
            Data = data;
        }

        /// <summary>
        /// Adds the given neighbor node to this node's neighbors.
        /// </summary>
        /// <param name="neighbor"></param>
        public void AddNeighbor(Node<T> neighbor)
        {
            if (neighbor == this)
            {
                throw new ArgumentException("Self-loops are not allowed. A node cannot be it's own neighbor.");
            }

            Neighbors.Add(neighbor);
        }

        /// <summary>
        /// Removes the given node from this node's neighbors.
        /// No exception is thrown if the given neighbor is not found.
        /// </summary>
        /// <param name="neighbor"></param>
        /// <returns></returns>
        public bool RemoveNeighbor(Node<T> neighbor)
        {
            return Neighbors.Remove(neighbor);
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
