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
using PiCross.Controls;

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

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
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

        public class PuzzleDataAdapter : IPuzzleData
        {
            private IViewModel viewModel;

            public PuzzleDataAdapter(IViewModel viewModel)
            {
                this.viewModel = viewModel;
            }

            public IGrid<object> Grid
            {
                get { return viewModel.Grid; }
            }

            public ISequence<object> ColumnConstraints
            {
                get { return viewModel.ColumnConstraints; }
            }

            public ISequence<object> RowConstraints
            {
                get { return viewModel.RowConstraints; }
            }
        }
    }

    public class PuzzleDataConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if ( value == null )
            {
                return null;
            }
            else
            {
                var vm = (EditorControl.IViewModel) value;

                return new EditorControl.PuzzleDataAdapter( vm );
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
