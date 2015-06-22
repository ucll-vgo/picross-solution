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
    [TemplateVisualState( GroupName = "FillStates", Name = "Unknown" )]
    [TemplateVisualState( GroupName = "FillStates", Name = "Empty" )]
    [TemplateVisualState( GroupName = "FillStates", Name = "Filled" )]
    [TemplateVisualState( GroupName = "FillStates", Name = "NoContents" )]
    public class PuzzleGridSquareControl : SquareControl
    {
        static PuzzleGridSquareControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata( typeof( PuzzleGridSquareControl ), new FrameworkPropertyMetadata( typeof( PuzzleGridSquareControl ) ) );
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateVisualState( false );
        }

        #region Contents

        public Square Contents
        {
            get { return (Square) GetValue( ContentsProperty ); }
            set { SetValue( ContentsProperty, value ); }
        }

        public static readonly DependencyProperty ContentsProperty =
            DependencyProperty.Register( "Contents", typeof( Square ), typeof( PuzzleGridSquareControl ), new PropertyMetadata( null, (obj, args) => ((PuzzleGridSquareControl) obj).OnContentsChanged(args) ) );

        private void OnContentsChanged(DependencyPropertyChangedEventArgs args)
        {
            UpdateFillState();
        }

        #endregion

        #region Visual States

        private void UpdateVisualState( bool transition = true )
        {
           
            UpdateFillState( transition );
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

        #endregion
    }

    public interface IPuzzleGridSquareViewModel
    {
        Cell<Square> Contents { get; }
    }
}
