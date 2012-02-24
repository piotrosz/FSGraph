using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GraphSharp.Controls;
using fsgraph.WPF.GraphModel;

namespace fsgraph.WPF.ViewModel
{
    public class FSGraphLayout : GraphLayout<FSVertex, FSEdge, FSGraph> { }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string layoutAlgorithmType;
        private FSGraph graph;
        private string directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private List<string> layoutAlgorithmTypes;

        public void ReLayoutGraph()
        {
            graph = GraphCreator.Create(Directory);
            NotifyPropertyChanged("Graph");
        }

        public List<String> LayoutAlgorithmTypes
        {
            get { return layoutAlgorithmTypes; }
        }

        public string LayoutAlgorithmType
        {
            get { return layoutAlgorithmType; }
            set
            {
                layoutAlgorithmType = value;
                NotifyPropertyChanged("LayoutAlgorithmType");
            }
        }

        public FSGraph Graph
        {
            get { return graph; }
            set
            {
                graph = value;
                NotifyPropertyChanged("Graph");
            }
        }

        public string Directory
        {
            get { return directory; }
            set
            {
                directory = value;
                NotifyPropertyChanged("Directory");
            }
        }

        public MainWindowViewModel()
        {
            //Graph = GraphCreator.Create(Directory);

            layoutAlgorithmTypes = new List<string>()
            {
                "BoundedFR",
                "Circular",
                "CompoundFDP",
                "EfficientSugiyama",
                "FR",
                "ISOM",
                "KK",
                "LinLog",
                "Tree"
            };

            // default
            LayoutAlgorithmType = "Tree";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
