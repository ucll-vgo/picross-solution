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
using GUI.Views;
using Cells;
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

        private readonly ICommand toggleFullScreen;

        private readonly ThemeManager themeManager;

        private readonly Cell<int> currentThemeIndex;

        public MainWindow()
        {
            InitializeComponent();

            this.masterController = CreateMasterController();
            this.toggleConsole = EnabledCommand.FromDelegate( PerformToggleConsole );
            this.toggleFullScreen = EnabledCommand.FromDelegate( PerformToggleFullScreen );
            this.themeManager = new ThemeManager();
            this.currentThemeIndex = Cell.Create( 0 );

            SetUpDataContext();
            SetUpTimer();
            SetUpTheme();
        }

        private MasterController CreateMasterController()
        {
            return new MasterController( this.Close );
        }

        private void SetUpDataContext()
        {
            Debug.Assert( masterController != null );

            this.DataContext = this;
            viewContainer.DataContext = masterController;
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

        public ICommand ToggleFullScreen
        {
            get
            {
                return toggleFullScreen;
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

        private void PerformToggleFullScreen()
        {
            if ( IsFullScreen )
            {
                MakeWindowed();
            }
            else
            {
                MakeFullScreen();
            }
        }

        private bool IsFullScreen
        {
            get
            {
                return this.WindowStyle == System.Windows.WindowStyle.None;
            }
        }

        private void MakeFullScreen()
        {
            this.WindowState = System.Windows.WindowState.Maximized;
            this.WindowStyle = System.Windows.WindowStyle.None;
        }

        private void MakeWindowed()
        {
            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
        }

        public IEnumerable<Theme> Themes
        {
            get
            {
                return themeManager.Themes;
            }
        }

        private void SetUpTheme()
        {
            this.currentThemeIndex.ValueChanged += UpdateTheme;
        }

        private void UpdateTheme()
        {
            var theme = this.themeManager.Themes[currentThemeIndex.Value];            

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add( themeManager.DefaultTheme.ResourceDictionary );
            Application.Current.Resources.MergedDictionaries.Add( theme.ResourceDictionary );
        }

        public Cell<int> CurrentThemeIndex
        {
            get
            {
                return this.currentThemeIndex;
            }
        }
    }
}
