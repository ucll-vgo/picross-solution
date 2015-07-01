using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using GUI.Commands;
using GUI.Controls;
using GUI.ViewModels;
using GUI.ViewModels.EditMode;
using GUI.ViewModels.LibraryMode;
using GUI.ViewModels.PuzzleMode;
using GUI.Views.PlayerSelection;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Editing;
using PiCross.Facade.IO;
using PiCross.Facade.Playing;
using PiCross.Game;
using Grid = PiCross.DataStructures.Grid;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MasterController();
        }
    }
}
