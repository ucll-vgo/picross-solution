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
using PiCross.Cells;
using PiCross.Game;

namespace GUI.Controls
{
    [TemplateVisualState( GroupName = "FillStates", Name = "Empty" )]
    [TemplateVisualState( GroupName = "FillStates", Name = "Filled" )]
    public class EditorGridSquareControl : SquareControl
    {
        static EditorGridSquareControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata( typeof( EditorGridSquareControl ), new FrameworkPropertyMetadata( typeof( EditorGridSquareControl ) ) );
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateVisualState( false );
        }

        #region Contents

        public bool Contents
        {
            get { return (bool) GetValue( ContentsProperty ); }
            set { SetValue( ContentsProperty, value ); }
        }

        public static readonly DependencyProperty ContentsProperty =
            DependencyProperty.Register( "Contents", typeof( bool ), typeof( EditorGridSquareControl ), new PropertyMetadata( false, ( obj, args ) => ( (EditorGridSquareControl) obj ).OnContentsChanged( args ) ) );

        private void OnContentsChanged( DependencyPropertyChangedEventArgs args )
        {
            UpdateVisualState( true );
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

            if ( contents )
            {
                ChangeVisualState( "Filled", transition );
            }
            else
            {
                ChangeVisualState( "Empty", transition );
            }
        }

        #endregion
    }
}
