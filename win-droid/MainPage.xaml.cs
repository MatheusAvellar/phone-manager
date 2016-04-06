using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.Devices.Power;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace win_droid
{
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        int counter = 0;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += timer_Tick;
            timer.Start();

            var _dpi = Windows.Graphics.Display.DisplayInformation.GetForCurrentView();
            if (_dpi != null) {
                this.DPIX.Text = _dpi.RawDpiX.ToString();
                this.DPIY.Text = _dpi.RawDpiY.ToString();
            }
        }

        void timer_Tick(object sender, object e) {

            var _compass = Windows.Devices.Sensors.Compass.GetDefault();
            if (_compass != null) {
                double angle = Math.Floor((double)_compass.GetCurrentReading().HeadingTrueNorth);
                this.CompassTrue.Text = angle + "°";
                this.CompassRotation.Rotation = angle * -1;
            }

            var _incline = Windows.Devices.Sensors.Inclinometer.GetDefault();
            if (_incline != null) {
                var currentInclination = _incline.GetCurrentReading();
                this.InclinometerX.Text = (Math.Truncate(currentInclination.PitchDegrees * 100) / 100).ToString() + "°";
                this.InclinometerY.Text = (Math.Truncate(currentInclination.RollDegrees * 100) / 100).ToString() + "°";
                this.InclinometerZ.Text = (Math.Truncate(currentInclination.YawDegrees * 100) / 100).ToString() + "°";
            }
            //var _light = Windows.Devices.Sensors.LightSensor.GetDefault();
            //if (_light != null) {
                //double lumen = Math.Truncate(_light.GetCurrentReading().IlluminanceInLux * 1000) / 1000;
                //this.GyroscopeX.Text = lumen.ToString() + " lumen";
                /*var GyroReading = _gyroscope.GetCurrentReading().AngularVelocityX;
                this.GyroscopeX.Text = String.Format("{0,5:0.00}", GyroReading);
                this.GyroscopeX.Text = _gyroscope.GetCurrentReading().AngularVelocityX.ToString();
                this.GyroscopeY.Text = _gyroscope.GetCurrentReading().AngularVelocityY.ToString();*/
            //}
            
            counter++;
            DateTime dt = DateTime.Now;
            CultureInfo ci = CultureInfo.InvariantCulture;

            this.Time.Text = dt.ToString("HH:mm", ci);
            this.Date.Text = dt.DayOfWeek.ToString() + '\n' + dt.ToString("MMMM d", ci);

            var Bat = Battery.GetDefault();
            var batPer = Bat.RemainingChargePercent;
            var batRem = Bat.RemainingDischargeTime;
            this.BatteryBar.Value = batPer;
            this.BatteryPercentage.Text = batPer.ToString() + "%";

            string savingModeActive = Windows.Phone.System.Power.PowerManager.PowerSavingMode.ToString();
            bool savingModeEnabled = Windows.Phone.System.Power.PowerManager.PowerSavingModeEnabled;
            string savingIsActive = (savingModeActive == "On") ? "active" : "inactive";
            string savingIsEnabled = (savingModeEnabled) ? "On" : "Off";
            this.BatterySaving.Text = "Battery Saving Mode: " + savingIsEnabled + ", " + savingIsActive;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
    }
}
