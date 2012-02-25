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

                if (dirInfo.IsHidden())
                {
                    var vertex1 = graph.Vertices.SingleOrDefault(v => v.FullName == dirInfo.FullName);

                    if (vertex1 == null)
                        vertex1 = new FSVertex(dirInfo.Name, dirInfo.FullName, dirInfo.GetFriendlySize(), VertexType.Directory,
                            dirInfo.CreationTime, dirInfo.LastAccessTime, dirInfo.LastWriteTime);

                    graph.AddVertex(vertex1);

                    string[] subDirs = null;
                    try
                    {
                        subDirs = Directory.GetDirectories(currentDir);

                        foreach (string dir in subDirs)
                        {
                            var dirInfo2 = new DirectoryInfo(dir);
                            if (dirInfo2.IsHidden())
                            {
                                var vertex2 = graph.Vertices.SingleOrDefault(v => v.FullName == dirInfo2.FullName);

                                if (vertex2 == null)
                                    vertex2 = new FSVertex(dirInfo2.Name, dirInfo2.FullName, dirInfo2.GetFriendlySize(), VertexType.Directory,
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
                    catch (UnauthorizedAccessException) {}
                    catch (DirectoryNotFoundException) {}

                    if (files != null)
                    {
                        foreach (string file in files)
                        {
                            try
                            {
                                var fileInfo = new FileInfo(file);

                                var vertex2 = graph.Vertices.SingleOrDefault(v => v.FullName == fileInfo.FullName);

                                if (vertex2 == null)
                                    vertex2 = new FSVertex(fileInfo.Name, fileInfo.FullName, fileInfo.GetFriendlySize(), VertexType.File,
                                        fileInfo.CreationTime, fileInfo.LastAccessTime, fileInfo.LastWriteTime);

                                graph.AddVertex(vertex2);
                                graph.AddEdge(new FSEdge(vertex1, vertex2));
                            }
                            catch (FileNotFoundException) {}
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
    }
}
