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
using PiCross.Game;

namespace GUI.Controls
{
    [TemplatePart( Name = "Body", Type = typeof( FrameworkElement ) )]
    [TemplateVisualState( GroupName = "MouseStates", Name = "MouseOver" )]
    [TemplateVisualState( GroupName = "MouseStates", Name = "MouseNotOver" )]
    [TemplateVisualState( GroupName = "FillStates", Name = "Unknown" )]
    [TemplateVisualState( GroupName = "FillStates", Name = "Empty" )]
    [TemplateVisualState( GroupName = "FillStates", Name = "Filled" )]
    public class GridSquareControl : Control
    {
        private FrameworkElement body;

        static GridSquareControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata( typeof( GridSquareControl ), new FrameworkPropertyMetadata( typeof( GridSquareControl ) ) );
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            body = (FrameworkElement) GetTemplateChild( "Body" );

            UpdateVisualState( false );
        }

        #region ViewModel

        public IPuzzleGridSquareViewModel ViewModel
        {
            get { return (IPuzzleGridSquareViewModel) GetValue( ViewModelProperty ); }
            set { SetValue( ViewModelProperty, value ); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel",
            typeof( IPuzzleGridSquareViewModel ),
            typeof( GridSquareControl ),
            new PropertyMetadata( null, ( obj, args ) => ( (GridSquareControl) obj ).OnViewModelChanged( args ) ) );

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

        #endregion

        #region LeftClick

        public ICommand LeftClick
        {
            get { return (ICommand) GetValue( LeftClickProperty ); }
            set { SetValue( LeftClickProperty, value ); }
        }

        public static readonly DependencyProperty LeftClickProperty =
            DependencyProperty.Register( "LeftClick", typeof( ICommand ), typeof( GridSquareControl ), new PropertyMetadata( null ) );

        #endregion

        #region RightClick

        public ICommand RightClick
        {
            get { return (ICommand) GetValue( RightClickProperty ); }
            set { SetValue( RightClickProperty, value ); }
        }

        public static readonly DependencyProperty RightClickProperty =
            DependencyProperty.Register( "RightClick", typeof( ICommand ), typeof( GridSquareControl ), new PropertyMetadata( null ) );

        #endregion

        #region Activate

        public ICommand Activate
        {
            get { return (ICommand) GetValue( ActivateProperty ); }
            set { SetValue( ActivateProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for Activate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivateProperty =
            DependencyProperty.Register( "Activate", typeof( ICommand ), typeof( GridSquareControl ), new PropertyMetadata( null ) );

        private void ExecuteActivateCommand()
        {
            if ( Activate != null && Activate.CanExecute( null ) )
            {
                Activate.Execute( null );
            }
        }

        #endregion

        #region Visual States

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
                else if ( contents == Square.UNKNOWN )
                {
                    ChangeVisualState( "Unknown", transition );
                }
                else
                {
                    throw new InvalidOperationException( "Unknown state" );
                }
            }
        }

        private void ChangeVisualState( string state, bool transition = true )
        {
            if ( body != null )
            {
                VisualStateManager.GoToElementState( body, state, transition );
            }
        }

        #endregion

        #region Events

        protected override void OnMouseEnter( MouseEventArgs e )
        {
            base.OnMouseEnter( e );

            UpdateMouseOverState();
            ExecuteActivateCommand();
        }

        protected override void OnMouseLeave( MouseEventArgs e )
        {
            base.OnMouseLeave( e );

            UpdateMouseOverState();
        }

        protected override void OnMouseLeftButtonDown( MouseButtonEventArgs e )
        {
            base.OnMouseLeftButtonDown( e );

            if ( LeftClick != null && LeftClick.CanExecute( null ) )
            {
                LeftClick.Execute( null );
            }
        }

        protected override void OnMouseRightButtonDown( MouseButtonEventArgs e )
        {
            base.OnMouseRightButtonDown( e );

            if ( RightClick != null && RightClick.CanExecute( null ) )
            {
                RightClick.Execute( null );
            }
        }

        #endregion
    }
}
