using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;

namespace fsgraph.WPF.GraphModel
{
    public class FSGraph : BidirectionalGraph<FSVertex, FSEdge>
    {
        public FSGraph() 
        { }

        public FSGraph(bool allowParallelEdges)
            : base(allowParallelEdges) 
        { }

         public FSGraph(bool allowParallelEdges, int vertexCapacity)
            : base(allowParallelEdges, vertexCapacity) { }
    }
}
