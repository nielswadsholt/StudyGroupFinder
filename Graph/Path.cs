using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Graph
{
    /// <summary>
    /// Represents a path between two nodes in a graph.
    /// </summary>
    public class Path
    {
        private LinkedList<string> nodes;
        public int Count => nodes.Count;
        
        public Path()
        {
            nodes = new LinkedList<string>();
        }

        public Path(string node)
        {
            nodes = new LinkedList<string>();
            Extend(node);
        }

        /// <summary>
        /// Adds a node to the path.
        /// </summary>
        /// <param name="node"></param>
        public void Extend(string node)
        {
            nodes.AddFirst(node);
        }

        public void Shorten()
        {
            nodes.RemoveFirst();
        }

        /// <summary>
        /// Returns the string representation of the path.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return nodes.ToSeparatedString(" -> ");
        }
    }
}
