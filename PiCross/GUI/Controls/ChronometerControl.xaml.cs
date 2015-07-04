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

namespace GUI.Controls
{
    /// <summary>
    /// Interaction logic for ChronometerControl.xaml
    /// </summary>
    public partial class ChronometerControl : UserControl
    {
        public ChronometerControl()
        {
            InitializeComponent();
        }

        public TimeSpan ElapsedTime
        {
            get { return (TimeSpan) GetValue( ElapsedTimeProperty ); }
            set { SetValue( ElapsedTimeProperty, value ); }
        }

        public static readonly DependencyProperty ElapsedTimeProperty =
            DependencyProperty.Register( "ElapsedTime", typeof( TimeSpan ), typeof( ChronometerControl ), new PropertyMetadata( TimeSpan.Zero, ( obj, args ) => ( (ChronometerControl) obj ).OnTimeElapsedChanged( args ) ) );

        private void OnTimeElapsedChanged( DependencyPropertyChangedEventArgs args )
        {
            UpdateHands();
        }

        private void UpdateHands()
        {
            UpdateSecondHand();
            UpdateMinuteHand();
        }

        private void UpdateSecondHand()
        {
            secondsTransform.Angle = TransformToAngle( ElapsedTime.Seconds + ElapsedTime.Milliseconds / 1000.0);
        }

        private void UpdateMinuteHand()
        {
            minutesTransform.Angle = TransformToAngle( ElapsedTime.Minutes );
        }

        private double TransformToAngle( double handPosition )
        {
            return handPosition * 6;
        }
    }
}
