using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using fsgraph.WPF.ViewModel;

namespace fsgraph.WPF
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;
        
        public MainWindow()
        {
            this.viewModel = new MainWindowViewModel();
            this.DataContext = this.viewModel;
            InitializeComponent();
        }

        private void btnCreateGraph_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.ReLayoutGraph();
        }

        private void btnChooseDir_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.ShowDialog();
            this.viewModel.Directory = dialog.SelectedPath;            
        }
    }
}
