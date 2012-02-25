using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fsgraph.WPF.GraphModel
{
    public enum VertexType
    {
        File,
        Directory
    }

    public class FSVertex
    {
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public string FriendlySize { get; private set; }
        public VertexType VertexType { get; private set; }
        public DateTime Created { get; set; }
        public DateTime Accessed { get; set; }
        public DateTime Modified { get; set; }

        public FSVertex(string name, string fullName, string friendlySize, VertexType vertexType,
            DateTime created, DateTime accessed, DateTime modified)
        {
            this.Name = name;
            this.FullName = fullName;
            this.FriendlySize = friendlySize;
            this.VertexType = vertexType;
            this.Created = created;
            this.Accessed = accessed;
            this.Modified = modified;
        }
    }
}
