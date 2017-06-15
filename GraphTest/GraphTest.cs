using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graph;
using System.Collections.Generic;
using StudyGroupFinder;
using Util;

namespace StudyGroupFinderTest
{
    [TestClass]
    public class GraphTest
    {
        [TestMethod]
        public void TestBFS()
        {
            // BFS constructor
            Console.WriteLine("\n========== Test: BFS ==========");
            var bfs = new BFS<string>();

            // FindPath() with tinyDG.txt
            Console.WriteLine("\n========== FindPath() with tinyDG.txt ==========");

            Console.WriteLine("\nGraph:");
            Digraph<int> intGraph = TestGraphLoader.LoadGraph(@"../../../StudyGroupFinder/Data/tinyDG.txt");
            Console.Write(intGraph);
            Console.WriteLine("Verify at http://algs4.cs.princeton.edu/42digraph/Digraph.java.html");

            Console.WriteLine("\nPaths:");
            var bfsInt = new BFS<int>();

            foreach (Node<int> node in intGraph.Nodes.Values)
            {
                Node<int> n = intGraph.Nodes["3"];
                // HACK: Add source-to-source path
                Path p = n == node ? new Path(node.ToString()) : bfsInt.FindPath(intGraph, n, n2 => intGraph.Nodes[n2.ToString()] == node);
                Console.WriteLine($"3 to {node.ToString()} ({p.Count - 1}):  " + p.ToString());
            }

            Console.WriteLine("Verify at http://algs4.cs.princeton.edu/42digraph/BreadthFirstDirectedPaths.java.html");

            // TEST: Add more verified BFS test samples
        }


        [TestMethod]
        public void TestDFS()
        {
            // DFS constructor
            Console.WriteLine("\n========== Test: DFS ==========");
            var dfs = new DFS<string>();

            // FindPath() with tinyDG.txt
            Console.WriteLine("\n========== FindPath() with tinyDG.txt ==========");

            Console.WriteLine("\nGraph:");
            Digraph<int> intGraph = TestGraphLoader.LoadGraph(@"../../../StudyGroupFinder/Data/tinyDG.txt");
            Console.Write(intGraph);
            Console.WriteLine("Verify at http://algs4.cs.princeton.edu/42digraph/Digraph.java.html");

            Console.WriteLine("\nPaths:");
            var dfsInt = new DFS<int>();

            foreach (Node<int> node in intGraph.Nodes.Values)
            {
                Node<int> n = intGraph.Nodes["3"];
                // HACK: Add source-to-source path
                Path p = n == node ? new Path(node.ToString()) : dfsInt.FindPath(intGraph, n, n2 => intGraph.Nodes[n2.ToString()] == node);
                Console.WriteLine($"3 to {node.ToString()} ({p.Count - 1}):  " + p.ToString());
            }

            Console.WriteLine("Verify at http://algs4.cs.princeton.edu/42digraph/DepthFirstDirectedPaths.java.html");

            // TEST: Add more verified DFS test samples
        }


        [TestMethod]
        public void TestDigraph()
        {
            // Digraph constructor
            Console.WriteLine("\n========== Test: Digraph ==========");
            var digraph = new Digraph<string>();

            // TEST: Indexer
            // TEST: Contains

            // Add nodes
            Console.WriteLine("\n========== Add nodes ==========");

            foreach (string name in new []{ "Hans", "Grethe", "Anne", "Ida", "Rikke", "Sofia", "Ben" })
            {
                digraph.AddNode(new Node<string>(name));
            }

            Console.Write(digraph);

            // Insert duplicate
            Console.WriteLine("\n========== Add duplicate node (throws ArgumentException) ==========");

            try
            {
                digraph.AddNode(new Node<string>("Hans"));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
            }

            Console.Write(digraph);

            // Remove node
            Console.WriteLine("\n========== Remove node ==========");
            digraph.RemoveNode("Sofia");
            Console.Write(digraph);

            Console.WriteLine("\n========== Remove non-existing node (no exception thrown) ==========");
            digraph.RemoveNode("Konfutse");
            Console.Write(digraph);

            // Indexer
            Console.WriteLine("\n========== Indexer ==========");
            Console.WriteLine(digraph["Anne"]);

            // Add edge
            Console.WriteLine("\n========== Add edge ==========");
            digraph.AddEdge("Grethe", "Anne");
            digraph.AddEdge("Anne", "Ben");
            digraph.AddEdge("Ben", "Ida");
            digraph.AddEdge("Ben", "Rikke");
            digraph.AddEdge("Rikke", "Ida");
            digraph.AddEdge("Rikke", "Ben");
            digraph.AddEdge("Grethe", "Hans");
            Console.Write(digraph);

            // Add existing edge (no exception thrown)
            Console.WriteLine("\n========== Add existing edge (no exception thrown) ==========");
            digraph.AddEdge("Grethe", "Anne");
            Console.Write(digraph);

            // Add edge between non-existing nodes (throws KeyNotFoundException)
            Console.WriteLine("\n========== Add edge between non-existing nodes (throws KeyNotFoundException) ==========");

            try
            {
                digraph.AddEdge("non-existing1", "non-existing2");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
            }
            Console.Write(digraph);

            // Remove edge
            Console.WriteLine("\n========== Remove edge ==========");
            digraph.RemoveEdge("Rikke", "Ben");
            Console.Write(digraph);

            // Remove non-existing edge
            Console.WriteLine("\n========== Remove non-existing edge (no exception thrown) ==========");
            digraph.RemoveEdge("Rikke", "Ben");
            Console.Write(digraph);

            // Remove edge between non-existing nodes (throws KeyNotFoundException)
            Console.WriteLine("\n========== Remove edge between non-existing nodes ==========");

            try
            {
                digraph.RemoveEdge("non-existing1", "non-existing2");
                Console.Write(digraph);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
            }

            try
            {
                digraph.RemoveEdge("Hans", "non-existing2");
                Console.Write(digraph);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
            }

            try
            {
                digraph.RemoveEdge("non-existing1", "Hans");
                Console.Write(digraph);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
            }

            // FindPath()
            Console.WriteLine("\n========== FindPath() ==========");
            Console.WriteLine(digraph.FindPath(digraph["Grethe"], n => n == "Ida").ToString());
            Console.WriteLine(digraph.FindPath("Grethe", n => n == "Ida").ToString());
            Console.WriteLine(digraph.FindPath(digraph["Grethe"], digraph["Ida"]).ToString());
            Console.WriteLine(digraph.FindPath("Grethe", "Ida").ToString());

            // FindShortestPath() with tinyDG.txt
            Console.WriteLine("\n========== FindShortestPath() with tinyDG.txt ==========");
            Digraph<int> intGraph = TestGraphLoader.LoadGraph(@"../../../StudyGroupFinder/Data/tinyDG.txt");
            string source = "3";

            foreach (Node<int> node in intGraph.Nodes.Values)
            {
                string dest = node.ToString();
                // HACK: Add source-to-source path
                Path p = source == dest ? new Path(source) : intGraph.FindShortestPath(source, dest);
                Console.WriteLine($"3 to {node.ToString()}:  " + p);
            }

            Console.WriteLine("Verify at http://algs4.cs.princeton.edu/42digraph/BreadthFirstDirectedPaths.java.html");

            // FindPathsToAll()
            Console.WriteLine("\n========== FindPathsToAll() with BFS and tinyDG.txt ==========");
            source = "3";

            foreach (KeyValuePair<string, string> dest in intGraph.FindPathsToAll(source))
            {
                // HACK: Add source-to-source path
                Console.WriteLine($"{source} to {dest.Key}:  " + (dest.Key == source ? source : dest.Value));
            }

            Console.WriteLine("Verify at http://algs4.cs.princeton.edu/42digraph/BreadthFirstDirectedPaths.java.html");

            // Change search method
            Console.WriteLine("\n========== Change search method ==========");
            Console.WriteLine("Old search method: " + intGraph.PathFinder.GetType());
            intGraph.PathFinder = new DFS<int>();
            Console.WriteLine("New search method: " + intGraph.PathFinder.GetType());

            // FindPathsToAll() with DFS
            Console.WriteLine("\n========== FindPathsToAll() with DFS and tinyDG.txt ==========");
            source = "3";

            foreach (KeyValuePair<string, string> dest in intGraph.FindPathsToAll(source))
            {
                // HACK: Add source-to-source path
                Console.WriteLine($"{source} to {dest.Key}:  " + (dest.Key == source ? source : dest.Value));
            }

            Console.WriteLine("Verify at http://algs4.cs.princeton.edu/42digraph/DepthFirstDirectedPaths.java.html");

            // Change search method to null
            Console.WriteLine("\n========== Change search method to null ==========");

            try
            {
                digraph.PathFinder = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
            }

            // ToString() with tinyDG.txt
            Console.WriteLine("\n========== ToString() with tinyDG.txt ==========");
            Console.Write(intGraph);
            Console.WriteLine("Verify at http://algs4.cs.princeton.edu/42digraph/Digraph.java.html");
        }


        [TestMethod]
        public void TestNode()
        {
            // Node constructor
            Console.WriteLine("\n========== Test: Node ==========");
            var node = new Node<string>("node");
            var nodeInt = new Node<int>(0);

            // Add neighbor
            Console.WriteLine("\n========== Add neighbor ==========");
            var neighbor = new Node<string>("neighbor1");
            node.AddNeighbor(neighbor);
            Assert.IsTrue(node.Neighbors.Contains(neighbor));

            node.AddNeighbor(new Node<string>("neighbor2"));
            node.AddNeighbor(new Node<string>("neighbor3"));

            // Self-loop (throws ArgumentException)
            Console.WriteLine("\n========== Self-loop (throws ArgumentException) ==========");

            try
            {
                node.AddNeighbor(node);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
            }

            // Remove neighbor
            Console.WriteLine("\n========== Remove neighbor ==========");

            Console.Write("Neighbors before: ");
            Console.WriteLine(node.Neighbors.ToSeparatedString(" "));

            node.RemoveNeighbor(neighbor);
            Assert.IsFalse(node.Neighbors.Contains(neighbor));

            Console.Write("Neighbors after: ");
            Console.WriteLine(node.Neighbors.ToSeparatedString(" "));

            // Remove non-existing neighbor (no exception thrown)
            node.RemoveNeighbor(new Node<string>("non-existing"));

            // ToString()
            Console.WriteLine("\n========== ToString() ==========");
            Console.WriteLine(node);
            Assert.AreEqual("node", node.ToString());

            // Check for null
            Console.WriteLine("\n========== Check for null ==========");
            var node2 = new Node<string>("node2");
            Console.WriteLine(node2.Neighbors);
            Console.WriteLine(node2.Neighbors.Count);
            Console.WriteLine(node2.Data);
        }


        [TestMethod]
        public void TestPath()
        {
            // TEST: Write tests for Path
        }
    }
}
