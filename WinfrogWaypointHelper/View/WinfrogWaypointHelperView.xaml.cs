using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WinfrogWayPointHelper.View;
using Xceed.Wpf.Toolkit;
using WindowState = System.Windows.WindowState;

namespace WinfrogWaypointHelper.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WinfrogWaypointHelperView : Window
    {

        // bool to enable functionality where if window is maximized and user click-drags, window is restored to previous size
        private bool _restoreIfMove;

        private readonly BitmapImage _restoreButtonImage = new BitmapImage(new Uri("/WinfrogWaypointHelper;component/Resources/RestoreButton.png", UriKind.Relative));
        private readonly BitmapImage _maximizeButtonImage = new BitmapImage(new Uri("/WinfrogWaypointHelper;component/Resources/MaximizeButton.png", UriKind.Relative));

        private bool _isFirstFocus = true;

        public WinfrogWaypointHelperView()
        {
            InitializeComponent();
            SetUpColors();
            SetUpCombobox();
        }

        private void ClearEntryTextBox(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_isFirstFocus && EntryTextBox != null)
            {
                EntryTextBox.Text = "";
                _isFirstFocus = false;
            }
                
        }

        private void EntryTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (EntryTextBox != null && ResultsTextBox != null)
            ConvertData();
        }

        private void ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (EntryTextBox != null && ResultsTextBox != null)
                ConvertData();
        }

        private void ConvertData()
        {
            var toConvert = EntryTextBox.Text;
            List<string> toConvertList = new List<string>(Regex.Split(toConvert, Environment.NewLine));

            ResultsTextBox.Text = "";

            foreach (var p in toConvertList)
            {
                List<string> items = p.Split(',').ToList<string>();
                if (items.Count < 2) continue;

                var eastingsString = "";
                var northingsString = "";
                var nameString = "";

                if (items.Count == 2)
                {
                    // just easting & northing 
                    eastingsString = items[0];
                    northingsString = items[1];
                }
                else if (items.Count == 3)
                {
                    nameString = items[0];
                    eastingsString = items[1];
                    northingsString = items[2];
                }
                else
                {
                    continue;
                }

                var iconString = IconComboBox.SelectedIndex;

                var colorString = WaypointColorPicker.SelectedColor.ToString();
                colorString = colorString.Substring(3, 6);
                var colorDecimalInt = int.Parse(colorString, System.Globalization.NumberStyles.HexNumber);
                var radiusString = RadiusTextBox.Text;
                var depthString = DepthTextBox.Text;


                //Name,Lat,Long,Shape(17 is default),Radius,Northing,Easting,Colour,Elevation
                ResultsTextBox.Text = ResultsTextBox.Text + nameString + ",,," + iconString + "," + radiusString + "," + northingsString + "," + eastingsString + "," + colorDecimalInt + "," + depthString + Environment.NewLine;

            }

            ResultsTextBox.Text = ResultsTextBox.Text + " ";
        }

        private void RadiusChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            if (EntryTextBox != null && ResultsTextBox != null)
                ConvertData();
        }

        private void DepthChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            if (EntryTextBox != null && ResultsTextBox != null)
                ConvertData();
        }

        private void IconChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EntryTextBox != null && ResultsTextBox != null)
                ConvertData();
        }

        private async void CopyToClipboardClicked(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ResultsTextBox.Text);
            StatusBarText.Text = "Copied to Clipboard";

            await Task.Run(async() => await ClearStatusBarTextAsync(1000));

            //Xceed.Wpf.Toolkit.MessageBox.Show("Copied to clipboard!");
        }

        private async Task ClearStatusBarTextAsync(int milliseconds)
        {
            Thread.Sleep(milliseconds);

            Application.Current.Dispatcher.Invoke(() =>
            {
                StatusBarText.Text = "";
            });
            
        }

        private void SetUpColors()
        {
            
            var customAvailableColors = new ObservableCollection<ColorItem>();

            // Set up all the "Standard colors" Winforg usually shows

            customAvailableColors.Add(new ColorItem(Color.FromRgb(255, 128, 128), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(255, 255, 128), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 255, 128), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 255, 128), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 255, 255), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 128, 255), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(255, 128, 192), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(255, 128, 255), ""));

            customAvailableColors.Add(new ColorItem(Color.FromRgb(255, 0, 0), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(255, 255, 0), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 255, 0), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 255, 64), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 255, 255), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 128, 192), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 128, 192), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(255, 0, 255), ""));

            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 64, 64), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(255, 128, 64), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 255, 0), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 128, 128), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 64, 128), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 128, 255), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 0, 64), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(255, 0, 128), ""));

            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 0, 0), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(255, 128, 0), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 128, 0), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 128, 64), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 0, 255), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 0, 160), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 0, 128), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 0, 255), ""));

            customAvailableColors.Add(new ColorItem(Color.FromRgb(64, 0, 0), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 64, 0), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 64, 0), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 64, 64), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 0, 128), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 0, 64), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(64, 0, 64), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(64, 0, 128), ""));

            customAvailableColors.Add(new ColorItem(Color.FromRgb(0, 0, 0), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 128, 0), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 128, 64), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(128, 128, 128), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(64, 128, 128), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(192, 192, 192), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(64, 0, 64), ""));
            customAvailableColors.Add(new ColorItem(Color.FromRgb(255, 255, 255), ""));

            WaypointColorPicker.AvailableColors = customAvailableColors;

            WaypointColorPicker.ShowStandardColors = false;
            WaypointColorPicker.SelectedColor = Color.FromRgb(0, 255, 0);
        }

        private void SetUpCombobox()
        {
            var iconOptions = new string[]
            {
                "Anchor",
                "Beacon - Radio/Radar",
                "Beacon - Square",
                "Beacon - Triangular",
                "Bullseye",
                "Circle",
                "Light - Directional",
                "Light - General",
                "Marker - Barrel Buoy",
                "Marker - BiColor",
                "Marker - General Buoy",
                "Marker - Spherical Buoy",
                "Mine",
                "Point",
                "Post/Pile",
                "Rock",
                "Sewer/Pipeline",
                "Square",
                "Triangle",
                "Wreck"
            };

            foreach (var s in iconOptions)
            {
                IconComboBox.Items.Add(s);
            }

            IconComboBox.SelectedItem = "Square";
        }

        private void AboutClicked(object sender, RoutedEventArgs e)
        {
            string about = "Winfrog Waypoint Helper by Duncan Tait, created 2016.\n" +
                           "View project on GitHub: www.github.com/dunctait/WinfrogWaypointHelper\n" +
                           "\n" +
                           "To use, simply type/paste in your Eastings and Northings in the left pane (and optionally with a waypoint name," +
                           " or the program will make this blank). Then choose the various settings above that will be assigned to " +
                           "all of the waypoints. Hit the Copy To Clipboard button then paste this data into a Waypoint.wpt text" +
                           "file. Keep in mind Winfrog won't automatically update the Waypoints. The best approach is to go to " +
                           "File > Select Working Files then change the waypoint file to a blank file and back.";
            CustomDialog dialog = new CustomDialog(about, "About Winfrog Waypoint Helper", false);
            dialog.ShowDialog();
        }

        /// <summary>
        /// Minimize Button in title bar clicked
        /// </summary>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Restore Button in title bar clicked
        /// </summary>
        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchWindowState();
        }

        /// <summary>
        /// Closes the program
        /// </summary>
        public void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Handles Maximize/Restore logic (switches from maximized to normal and vice versa)
        /// </summary>
        private void SwitchWindowState()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    {
                        WindowState = WindowState.Maximized;
                        RestoreButtonImage.Source = _restoreButtonImage;
                        break;
                    }
                case WindowState.Maximized:
                    {
                        WindowState = WindowState.Normal;
                        RestoreButtonImage.Source = _maximizeButtonImage;
                        break;
                    }
            }
        }

        /// <summary>
        /// Enables double-clicking on title bar to maximize and also click-to-drag on title bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if ((ResizeMode == ResizeMode.CanResize) || (ResizeMode == ResizeMode.CanResizeWithGrip))
                {
                    SwitchWindowState();
                }

                return;
            }

            if (WindowState == WindowState.Maximized)
            {
                _restoreIfMove = true;
                return;
            }

            DragMove();
        }

        private void TitleBarMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _restoreIfMove = false;
        }

        private void TitleBarMouseMove(object sender, MouseEventArgs e)
        {
            if (!_restoreIfMove) return;
            _restoreIfMove = false;

            var percentHorizontal = e.GetPosition(this).X / ActualWidth;
            var targetHorizontal = RestoreBounds.Width * percentHorizontal;

            var percentVertical = e.GetPosition(this).Y / ActualHeight;
            var targetVertical = RestoreBounds.Height * percentVertical;

            WindowState = WindowState.Normal;

            POINT lMousePosition;
            GetCursorPos(out lMousePosition);

            Left = lMousePosition.X - targetHorizontal;
            Top = lMousePosition.Y - targetVertical;

            DragMove();
        }

        #region MaximizeCode

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr mWindowHandle = (new WindowInteropHelper(this)).Handle;
            HwndSource.FromHwnd(mWindowHandle)?.AddHook(new HwndSourceHook(WindowProc));
        }

        private static System.IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    break;
            }

            return IntPtr.Zero;
        }

        private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            POINT lMousePosition;
            GetCursorPos(out lMousePosition);

            IntPtr lPrimaryScreen = MonitorFromPoint(new POINT(0, 0), MonitorOptions.MONITOR_DEFAULTTOPRIMARY);
            MONITORINFO lPrimaryScreenInfo = new MONITORINFO();
            if (GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo) == false)
            {
                return;
            }

            IntPtr lCurrentScreen = MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONEAREST);

            MINMAXINFO lMmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            if (lPrimaryScreen.Equals(lCurrentScreen) == true)
            {
                lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcWork.Left;
                lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcWork.Top;
                lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcWork.Right - lPrimaryScreenInfo.rcWork.Left;
                lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcWork.Bottom - lPrimaryScreenInfo.rcWork.Top;
            }
            else
            {
                lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcMonitor.Left;
                lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcMonitor.Top;
                lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcMonitor.Right - lPrimaryScreenInfo.rcMonitor.Left;
                lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcMonitor.Bottom - lPrimaryScreenInfo.rcMonitor.Top;
            }

            Marshal.StructureToPtr(lMmi, lParam, true);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);


        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

        private enum MonitorOptions : uint
        {
            MONITOR_DEFAULTTONULL = 0x00000000,
            MONITOR_DEFAULTTOPRIMARY = 0x00000001,
            MONITOR_DEFAULTTONEAREST = 0x00000002
        }


        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);


        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }
        }

        #endregion MaximizeCode
    }
}
