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
            new PropertyMetadata( null, ( obj, args ) => ( (PuzzleGridSquareControl) obj ).OnViewModelChanged( args ) ) );

        private void OnViewModelChanged( DependencyPropertyChangedEventArgs args )
        {
            if ( args.OldValue != null )
            {
                UnbindFromViewModel( (IPuzzleGridSquareViewModel) args.OldValue );
            }

            if ( args.NewValue != null )
            {
                BindToViewModel( (IPuzzleGridSquareViewModel) args.NewValue );
            }

            UpdateVisualState( false );
        }

        private void UnbindFromViewModel( IPuzzleGridSquareViewModel viewModel )
        {
            // TODO
        }

        private void BindToViewModel( IPuzzleGridSquareViewModel viewModel )
        {
            viewModel.Contents.ValueChanged += () => UpdateFillState();
        }

        public ICommand LeftClick
        {
            get { return (ICommand) GetValue( LeftClickProperty ); }
            set { SetValue( LeftClickProperty, value ); }
        }

        public static readonly DependencyProperty LeftClickProperty =
            DependencyProperty.Register( "LeftClick", typeof( ICommand ), typeof( PuzzleGridSquareControl ), new PropertyMetadata( null ) );

        public ICommand RightClick
        {
            get { return (ICommand) GetValue( RightClickProperty ); }
            set { SetValue( RightClickProperty, value ); }
        }

        public static readonly DependencyProperty RightClickProperty =
            DependencyProperty.Register( "RightClick", typeof( ICommand ), typeof( PuzzleGridSquareControl ), new PropertyMetadata( null ) );

        private void UpdateVisualState( bool transition = true )
        {
            UpdateMouseOverState( transition );
            UpdateFillState( transition );
        }

        private void UpdateMouseOverState( bool transition = true )
        {
            if ( IsMouseOver )
            {
                ChangeVisualState( "MouseOver", transition );
            }
            else
            {
                ChangeVisualState( "MouseNotOver", transition );
            }
        }

        private void UpdateFillState( bool transition = true )
        {
            if ( ViewModel != null )
            {
                var contents = this.ViewModel.Contents.Value;

                if ( contents == Square.EMPTY )
                {
                    ChangeVisualState( "Empty", transition );
                }
                else if ( contents == Square.FILLED )
                {
                    ChangeVisualState( "Filled", transition );
                }
                else
                {
                    ChangeVisualState( "Unknown", transition );
                }
            }
        }

        private void ChangeVisualState( string state, bool transition = true )
        {
            VisualStateManager.GoToElementState( this, state, transition );
        }

        private void OnMouseEnter( object sender, MouseEventArgs e )
        {
            UpdateMouseOverState();
        }

        private void OnMouseLeave( object sender, MouseEventArgs e )
        {
            UpdateMouseOverState();
        }

        private void OnMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            if ( LeftClick != null && LeftClick.CanExecute( null ) )
            {
                LeftClick.Execute( null );
            }
        }

        private void OnMouseRightButtonDown( object sender, MouseButtonEventArgs e )
        {
            if ( RightClick != null && RightClick.CanExecute( null ) )
            {
                RightClick.Execute( null );
            }
        }
    }

    public interface IPuzzleGridSquareViewModel
    {
        Cell<Square> Contents { get; }
    }
}
