using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FolderExplorer_
{
    /// <summary>
    /// Interaction logic for FolderControl.xaml
    /// </summary>
    public partial class FolderControl : UserControl
    {
        public MainWindow mainWindow;
        public DirItem dirItem = new DirItem();

        public FolderControl()
        {
            InitializeComponent();
            MouseEnter += (s, e) => Background = Brushes.Gray;
            MouseLeave += (s, e) => Background = Brushes.Transparent;
            MouseDown += (s, e) =>
            {
                if (e.ClickCount == 2)
                {
                    if (dirItem.canLaunch)
                    {
                        System.Diagnostics.Process.Start(dirItem.Name);
                    }
                    else 
                    { 
                        mainWindow.GetDirs(FolderName.Text);
                    }
                }
            };
        }
    }
}
