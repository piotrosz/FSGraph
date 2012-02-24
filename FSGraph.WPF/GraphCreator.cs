using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fsgraph.WPF.GraphModel;
using System.IO;
using QuickGraph;
using QuickGraph.Algorithms;

namespace fsgraph.WPF
{
    public class GraphCreator
    {
        public static FSGraph Create(string root)
        {
            var graph = new FSGraph();
            var dirs = new Stack<string>(20);

            if (!Directory.Exists(root))
            {
                throw new ArgumentException();
            }
            dirs.Push(root);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                var dirInfo = new DirectoryInfo(currentDir);

                if ((dirInfo.Attributes & FileAttributes.Hidden) == 0)
                {
                    var vertex1 = graph.Vertices.SingleOrDefault(v => v.FullName == dirInfo.FullName);

                    if (vertex1 == null)
                        vertex1 = new FSVertex(dirInfo.Name, dirInfo.FullName, GetDirectorySize(currentDir), VertexType.Directory,
                            dirInfo.CreationTime, dirInfo.LastAccessTime, dirInfo.LastWriteTime);

                    graph.AddVertex(vertex1);

                    string[] subDirs = null;
                    try
                    {
                        subDirs = Directory.GetDirectories(currentDir);

                        foreach (string dir in subDirs)
                        {
                            var dirInfo2 = new DirectoryInfo(dir);
                            if ((dirInfo2.Attributes & FileAttributes.Hidden) == 0)
                            {
                                var vertex2 = graph.Vertices.SingleOrDefault(v => v.FullName == dirInfo2.FullName);

                                if (vertex2 == null)
                                    vertex2 = new FSVertex(dirInfo2.Name, dirInfo2.FullName, GetDirectorySize(dir), VertexType.Directory,
                                        dirInfo2.CreationTime, dirInfo2.LastAccessTime, dirInfo2.LastWriteTime);

                                graph.AddVertex(vertex2);
                                var edge = new FSEdge(vertex1, vertex2);
                                graph.AddEdge(edge);
                            }
                        }
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (DirectoryNotFoundException) { }

                    string[] files = null;
                    try
                    {
                        files = Directory.GetFiles(currentDir);
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (DirectoryNotFoundException) { }

                    if (files != null)
                    {
                        foreach (string file in files)
                        {
                            try
                            {
                                var fileInfo = new FileInfo(file);

                                var vertex2 = graph.Vertices.SingleOrDefault(v => v.FullName == fileInfo.FullName);

                                if (vertex2 == null)
                                    vertex2 = new FSVertex(fileInfo.Name, fileInfo.FullName, fileInfo.Length, VertexType.File,
                                        fileInfo.CreationTime, fileInfo.LastAccessTime, fileInfo.LastWriteTime);

                                graph.AddVertex(vertex2);
                                graph.AddEdge(new FSEdge(vertex1, vertex2));
                            }
                            catch (FileNotFoundException)
                            {
                                continue;
                            }
                        }
                    }

                    if (subDirs != null)
                    {
                        foreach (string dir in subDirs)
                            dirs.Push(dir);
                    }
                }
            }
            return graph;
        }

        private static long GetDirectorySize(string path)
        {
            string[] files = Directory.GetFiles(path, "*.*");

            long totalBytes = 0;
            foreach (string name in files)
            {
                FileInfo fileInfo = new FileInfo(name);
                totalBytes += fileInfo.Length;
            }
            return totalBytes;
        }

        // TODO
        // degree distribution 
        public static void Statistics(FSGraph graph)
        {
            int totalDegree = 0;
            long totalSize = 0;
            int totalNameLength = 0;

            // Average path
            //Func<FSEdge, double> edgeCost = e => 1; // constant cost
            //FSVertex root = graph.Vertices.First();
            //var tryGetPaths = graph.ShortestPathsDijkstra(edgeCost, root);
            //FSVertex target = graph.Vertices.ElementAt(2);

            //IEnumerable<FSEdge> path;
            //tryGetPaths(target, out path);

            foreach (var vertex in graph.Vertices)
            {
                if(vertex.VertexType == VertexType.File)
                    totalSize += vertex.Size;
                totalNameLength += vertex.Name.Length;
                totalDegree += graph.Degree(vertex);
            }

            double avgDegree = ((double) totalDegree) / ((double) graph.VertexCount);
            double avgSize = ((double)totalSize) / ((double)graph.Vertices.Count(v => v.VertexType == VertexType.File));
            double avgFileLength = ((double)totalNameLength) / ((double)graph.VertexCount);
        }
    }
}
