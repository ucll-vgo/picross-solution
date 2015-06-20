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
using GUI.ViewModels.PuzzleMode;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Solving;
using PiCross.Game;
using Grid = PiCross.DataStructures.Grid;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICommand setTheme;

        public MainWindow()
        {
            InitializeComponent();

            var editorGrid = EditorGrid.FromStrings(
                ".xxxxxxxx.",
                "x........x",
                "x.x......x",
                "x.xxxxxx.x",
                "x......x.x",
                "x......x.x",
                "x.xxxxxx.x",
                "x.x......x",
                "x........x",
                ".xxxxxxxx."
                );

            var playGrid = editorGrid.CreatePlayGrid();
            // playGrid.Squares.Overwrite( editorGrid.Squares );
                        
            // this.DataContext = new EditorViewModel( new PiCross.Facade.Editing.( playGrid ) );

            setTheme = new SetThemeCommand();
            menu.DataContext = this;
        }

        public ICommand SetTheme
        {
            get
            {
                return setTheme;
            }
        }

        private class SetThemeCommand : EnabledCommand
        {
            public override void Execute( object parameter )
            {
                var dics = Application.Current.Resources.MergedDictionaries;

                dics.Clear();
                dics.Add( new ResourceDictionary() { Source = new Uri( string.Format( "pack://application:,,,/Themes/{0}.xaml", parameter ) ) } );
            }
        }
    }
}
