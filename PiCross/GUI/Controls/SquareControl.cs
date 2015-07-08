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

namespace GUI.Controls
{
    [TemplatePart( Name = "Body", Type = typeof( FrameworkElement ) )]
    [TemplateVisualState( GroupName = "MouseStates", Name = "MouseOver" )]
    [TemplateVisualState( GroupName = "MouseStates", Name = "MouseNotOver" )]
    [TemplateVisualState( GroupName = "SelectionStates", Name = "Selected" )]
    [TemplateVisualState( GroupName = "SelectionStates", Name = "NotSelected" )]
    public class SquareControl : Control
    {
        private FrameworkElement body;

        static SquareControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata( typeof( SquareControl ), new FrameworkPropertyMetadata( typeof( SquareControl ) ) );
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            body = (FrameworkElement) GetTemplateChild( "Body" );
            UpdateVisualState();
        }

        #region Selection

        public bool IsSelected
        {
            get { return (bool) GetValue( IsSelectedProperty ); }
            set { SetValue( IsSelectedProperty, value ); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register( "IsSelected", typeof( bool ), typeof( SquareControl ), new PropertyMetadata( false, ( obj, args ) => ( (SquareControl) obj ).OnIsSelectedChanged( args ) ) );

        private void OnIsSelectedChanged(DependencyPropertyChangedEventArgs args)
        {
            UpdateSelectionVisualState();
        }

        #endregion

        #region LeftClick

        public ICommand LeftClick
        {
            get { return (ICommand) GetValue( LeftClickProperty ); }
            set { SetValue( LeftClickProperty, value ); }
        }

        public static readonly DependencyProperty LeftClickProperty =
            DependencyProperty.Register( "LeftClick", typeof( ICommand ), typeof( SquareControl ), new PropertyMetadata( null ) );

        #endregion

        #region RightClick

        public ICommand RightClick
        {
            get { return (ICommand) GetValue( RightClickProperty ); }
            set { SetValue( RightClickProperty, value ); }
        }

        public static readonly DependencyProperty RightClickProperty =
            DependencyProperty.Register( "RightClick", typeof( ICommand ), typeof( SquareControl ), new PropertyMetadata( null ) );

        #endregion

        #region Activate

        public ICommand Activate
        {
            get { return (ICommand) GetValue( ActivateProperty ); }
            set { SetValue( ActivateProperty, value ); }
        }

        public static readonly DependencyProperty ActivateProperty =
            DependencyProperty.Register( "Activate", typeof( ICommand ), typeof( SquareControl ), new PropertyMetadata( null ) );

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
            UpdateSelectionVisualState( transition );
        }

        private void UpdateSelectionVisualState(bool transition = true)
        {
            if ( IsSelected )
            {
                ChangeVisualState( "Selected", transition );
            }
            else
            {
                ChangeVisualState( "NotSelected", transition );
            }
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

        protected void ChangeVisualState( string state, bool transition = true )
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
