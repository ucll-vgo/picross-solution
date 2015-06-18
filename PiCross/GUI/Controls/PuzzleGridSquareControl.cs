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
using PiCross.Cells;
using PiCross.Game;

namespace GUI.Controls
{
    [TemplatePart( Name = "Body", Type = typeof( FrameworkElement ) )]
    [TemplateVisualState( GroupName = "MouseStates", Name = "MouseOver" )]
    [TemplateVisualState( GroupName = "MouseStates", Name = "MouseNotOver" )]
    [TemplateVisualState( GroupName = "FillStates", Name = "Unknown" )]
    [TemplateVisualState( GroupName = "FillStates", Name = "Empty" )]
    [TemplateVisualState( GroupName = "FillStates", Name = "Filled" )]
    [TemplateVisualState( GroupName = "FillStates", Name = "NoContents" )]
    public class PuzzleGridSquareControl : Control
    {
        private FrameworkElement body;

        static PuzzleGridSquareControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata( typeof( PuzzleGridSquareControl ), new FrameworkPropertyMetadata( typeof( PuzzleGridSquareControl ) ) );
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            body = (FrameworkElement) GetTemplateChild( "Body" );

            UpdateVisualState( false );
        }

        #region Contents

        public Square Contents
        {
            get { return (Square) GetValue( ContentsProperty ); }
            set { SetValue( ContentsProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for Contents.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentsProperty =
            DependencyProperty.Register( "Contents", typeof( Square ), typeof( PuzzleGridSquareControl ), new PropertyMetadata( null, (obj, args) => ((PuzzleGridSquareControl) obj).OnContentsChanged(args) ) );

        private void OnContentsChanged(DependencyPropertyChangedEventArgs args)
        {
            UpdateFillState();
        }

        #endregion

        #region LeftClick

        public ICommand LeftClick
        {
            get { return (ICommand) GetValue( LeftClickProperty ); }
            set { SetValue( LeftClickProperty, value ); }
        }

        public static readonly DependencyProperty LeftClickProperty =
            DependencyProperty.Register( "LeftClick", typeof( ICommand ), typeof( PuzzleGridSquareControl ), new PropertyMetadata( null ) );

        #endregion

        #region RightClick

        public ICommand RightClick
        {
            get { return (ICommand) GetValue( RightClickProperty ); }
            set { SetValue( RightClickProperty, value ); }
        }

        public static readonly DependencyProperty RightClickProperty =
            DependencyProperty.Register( "RightClick", typeof( ICommand ), typeof( PuzzleGridSquareControl ), new PropertyMetadata( null ) );

        #endregion

        #region Activate

        public ICommand Activate
        {
            get { return (ICommand) GetValue( ActivateProperty ); }
            set { SetValue( ActivateProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for Activate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivateProperty =
            DependencyProperty.Register( "Activate", typeof( ICommand ), typeof( PuzzleGridSquareControl ), new PropertyMetadata( null ) );

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
            var contents = this.Contents;

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
                ChangeVisualState( "NoContents", transition );
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

    public interface IPuzzleGridSquareViewModel
    {
        Cell<Square> Contents { get; }
    }
}
