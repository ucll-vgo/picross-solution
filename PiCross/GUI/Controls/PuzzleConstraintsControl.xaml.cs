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
using PiCross.Cells;
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
        }

        #region Orientation

        public static readonly DependencyProperty OrientationProperty =
            StackPanel.OrientationProperty.AddOwner( typeof( PuzzleConstraintsControl ) );

        public Orientation Orientation
        {
            get { return (Orientation) GetValue( OrientationProperty ); }
            set { SetValue( OrientationProperty, value ); }
        }

        #endregion

        #region ValueTemplate

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

        #endregion

        #region ViewModel

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

            UnbindFromViewModel( (IPuzzleConstraintsViewModel) args.OldValue );
            BindToViewModel( (IPuzzleConstraintsViewModel) args.NewValue );

            UpdateSatisfactionVisualState();
        }

        private void UnbindFromViewModel( IPuzzleConstraintsViewModel viewModel )
        {
            // TODO
        }

        private void BindToViewModel( IPuzzleConstraintsViewModel viewModel )
        {
            viewModel.IsSatisfied.ValueChanged += UpdateSatisfactionVisualState;
        }

        #endregion

        #region IsHighlighted

        public bool IsHighlighted
        {
            get { return (bool) GetValue( IsHighlightedProperty ); }
            set { SetValue( IsHighlightedProperty, value ); }
        }

        public static readonly DependencyProperty IsHighlightedProperty =
            DependencyProperty.Register( "IsHighlighted", typeof( bool ), typeof( PuzzleConstraintsControl ), new PropertyMetadata( false, ( obj, args ) => ( (PuzzleConstraintsControl) obj ).OnIsHighlightedChanged( args ) ) );

        private void OnIsHighlightedChanged( DependencyPropertyChangedEventArgs args )
        {
            UpdateHighlightedVisualState();
        }

        #endregion

        #region Children

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

        #endregion

        #region Visual States

        private void UpdateVisualState()
        {
            UpdateSatisfactionVisualState();
            UpdateHighlightedVisualState();
        }

        private void UpdateSatisfactionVisualState()
        {
            if ( ViewModel != null )
            {
                if ( ViewModel.IsSatisfied.Value )
                {
                    ChangeVisualState( "Satisfied" );
                }
                else
                {
                    ChangeVisualState( "Unsatisfied" );
                }
            }
            else
            {
                ChangeVisualState( "Satisfaction_NoVM" );
            }
        }

        private void UpdateHighlightedVisualState()
        {
            if ( IsHighlighted )
            {
                ChangeVisualState( "Highlighted" );
            }
            else
            {
                ChangeVisualState( "Lowlighted" );
            }
        }

        private void ChangeVisualState( string state, bool transition = true )
        {            
            VisualStateManager.GoToElementState( this, state, transition );
        }

        #endregion
    }

    public interface IPuzzleConstraintsViewModel
    {
        ISequence<IPuzzleConstraintsValueViewModel> Values { get; }

        Cell<bool> IsSatisfied { get; }
    }
}
