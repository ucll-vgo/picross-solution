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
using Cells;
using DataStructures;
using PiCross;

namespace GUI.Controls
{
    /// <summary>
    /// Interaction logic for EditorControl.xaml
    /// </summary>
    public partial class EditorControl : UserControl
    {
        public EditorControl()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel) GetValue( ViewModelProperty ); }
            set { SetValue( ViewModelProperty, value ); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register( "ViewModel", typeof( IViewModel ), typeof( EditorControl ), new PropertyMetadata( null ) );

        public interface IViewModel
        {
            IGrid<ISquareViewModel> Grid { get; }

            ISequence<IConstraintsViewModel> ColumnConstraints { get; }

            ISequence<IConstraintsViewModel> RowConstraints { get; }

            IGrid<Cell<bool>> ThumbnailData { get; }
        }

        public interface ISquareViewModel
        {
            Cell<bool> IsFilled { get; }

            Cell<Ambiguity> Ambiguity { get; }

            ICommand Activate { get; }

            ICommand SetFilled { get; }

            ICommand SetEmpty { get; }
        }

        public interface IConstraintsViewModel
        {
            Cell<IEnumerable<int>> Constraints { get; }

            Cell<bool> IsActive { get; }
        }
    }
}
