using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using QuickGraph;

namespace fsgraph.WPF.GraphModel
{
    public class FSEdge : Edge<FSVertex>
    {
        public FSEdge(FSVertex source, FSVertex target)
            : base(source, target)
        {
        }
    }
}
