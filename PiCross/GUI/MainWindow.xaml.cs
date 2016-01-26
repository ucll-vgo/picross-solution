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
using System.Windows.Threading;
using GUI.Commands;
using GUI.Controls;
using GUI.Themes;
using GUI.ViewModels;
using GUI.ViewModels.EditMode;
using GUI.ViewModels.LibraryMode;
using GUI.ViewModels.PlayMode;
using GUI.Views;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Editing;
using PiCross.Facade.IO;
using PiCross.Facade.Playing;
using PiCross.Game;
using Grid = PiCross.DataStructures.Grid;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MasterController masterController;

        private readonly ICommand toggleConsole;

        public MainWindow()
        {
            InitializeComponent();

            masterController = CreateMasterController();
            toggleConsole = EnabledCommand.FromDelegate( PerformToggleConsole );

            SetUpDataContext();
            SetUpTimer();
        }

        private MasterController CreateMasterController()
        {
            return new MasterController( this.Close );
        }

        private void SetUpDataContext()
        {
            Debug.Assert( masterController != null );

            this.DataContext = masterController;
        }

        private void SetUpTimer()
        {
            Debug.Assert( masterController != null );

            var timer = new Timer();
            var dispatcherTimer = new DispatcherTimer( TimeSpan.FromMilliseconds( 50 ), DispatcherPriority.Background, ( sender, args ) => timer.OnTick(), this.Dispatcher );
            dispatcherTimer.Start();

            timer.Tick += ( dt ) => masterController.ActiveViewModel.Value.OnTick( dt );
        }

        public ICommand ToggleConsole
        {
            get
            {
                return toggleConsole;
            }
        }

        private void PerformToggleConsole()
        {
            switch ( consoleContainer.Visibility )
            {
                case System.Windows.Visibility.Collapsed:
                    consoleContainer.Visibility = System.Windows.Visibility.Visible;
                    break;

                case System.Windows.Visibility.Visible:
                    consoleContainer.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }

        public IEnumerable<Theme> Themes
        {
            get
            {
                return new ThemeManager().Themes;
            }
        }

        
    }
}
