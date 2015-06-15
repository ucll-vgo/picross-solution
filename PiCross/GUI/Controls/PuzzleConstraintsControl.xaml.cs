using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using PiCross.DataStructures;

namespace GUI.Controls
{
    /// <summary>
    /// Interaction logic for PuzzleConstraintsControl.xaml
    /// </summary>
    public partial class PuzzleConstraintsControl : UserControl
    {
        public PuzzleConstraintsControl()
        {
            InitializeComponent();

            // DependencyPropertyDescriptor.FromProperty( DataContextProperty, typeof( Control ) ).AddValueChanged( this, ( obj, sender ) => Debug.WriteLine( "Data context changed to {0}", this.DataContext ) );
        }

        public static readonly DependencyProperty OrientationProperty =
            StackPanel.OrientationProperty.AddOwner( typeof( PuzzleConstraintsControl ) );

        public Orientation Orientation
        {
            get { return (Orientation) GetValue( OrientationProperty ); }
            set { SetValue( OrientationProperty, value ); }
        }

        public DataTemplate ValueTemplate
        {
            get { return (DataTemplate) GetValue( ValueTemplateProperty ); }
            set { SetValue( ValueTemplateProperty, value ); }
        }

        public static readonly DependencyProperty ValueTemplateProperty =
            DependencyProperty.Register( "ValueTemplate", typeof( DataTemplate ), typeof( PuzzleConstraintsControl ), new PropertyMetadata( ( obj, args ) => ( (PuzzleConstraintsControl) obj ).OnValueTemplateChanged( args ) ) );

        private void OnValueTemplateChanged( DependencyPropertyChangedEventArgs args )
        {
            RecreateChildren();
        }

        public IPuzzleConstraintsViewModel ViewModel
        {
            get { return (IPuzzleConstraintsViewModel) GetValue( ViewModelProperty ); }
            set { SetValue( ViewModelProperty, value ); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register( "ViewModel", typeof( IPuzzleConstraintsViewModel ), typeof( PuzzleConstraintsControl ), new PropertyMetadata( ( obj, args ) => ( (PuzzleConstraintsControl) obj ).OnViewModelChanged( args ) ) );

        private void OnViewModelChanged( DependencyPropertyChangedEventArgs args )
        {
            RecreateChildren();
        }

        private void RecreateChildren()
        {
            ClearChildren();
            CreateChildren();
        }

        private void ClearChildren()
        {
            this.stackPanel.Children.Clear();
        }

        private void CreateChildren()
        {
            Debug.Assert( this.stackPanel.Children.Count == 0 );

            if ( ViewModel != null && ValueTemplate != null )
            {
                foreach ( var valueViewModel in ViewModel.Values.Items )
                {
                    var control = (FrameworkElement) ValueTemplate.LoadContent();

                    control.DataContext = valueViewModel;

                    this.stackPanel.Children.Add( control );
                }
            }
        }
    }

    public interface IPuzzleConstraintsViewModel
    {
        ISequence<IPuzzleConstraintsValueViewModel> Values { get; }

        Cell<bool> IsSatisfied { get; }
    }
}
