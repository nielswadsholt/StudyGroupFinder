using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    /// <summary>
    /// Defines a directed graph search algorithm with a method for finding a path between two nodes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPathFinder<T>
    {
        /// <summary>
        /// Returns a string representation of a path between a source node and a destination
        /// node that matches the given predicate.
        /// </summary>
        /// <param name="digraph"></param>
        /// <param name="source"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        Path FindPath(Digraph<T> digraph, Node<T> source, Predicate<T> match);
    }
}
