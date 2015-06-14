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
using PiCross.DataStructures;
using PiCross.Game;

namespace GUI.Controls
{
    /// <summary>
    /// Interaction logic for PuzzleGridSquareControl.xaml
    /// </summary>
    public partial class PuzzleGridSquareControl : UserControl
    {
        public PuzzleGridSquareControl()
        {
            InitializeComponent();
        }

        public IPuzzleGridSquareViewModel ViewModel
        {
            get { return (IPuzzleGridSquareViewModel) GetValue( ViewModelProperty ); }
            set { SetValue( ViewModelProperty, value ); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel",
            typeof( IPuzzleGridSquareViewModel ),
            typeof( PuzzleGridSquareControl ),
            new PropertyMetadata( null ) );
    }

    public interface IPuzzleGridSquareViewModel
    {
        ICell<Square> Contents { get; }
    }
}
