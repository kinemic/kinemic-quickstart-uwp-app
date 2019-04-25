using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Kinemic.Gesture;

namespace kinemic_quickstart_uwp_app
{

    public sealed partial class MainPage : Page
    {
        // our handle to the shared engine
        private Engine engine;

        // the current band
        private String band;

        public MainPage()
        {
            this.InitializeComponent();

            // get the shared engine instance
            this.engine = Engine.Default;

            // register event handlers
            this.engine.ConnectionStateChanged += Engine_ConnectionStateChanged;
            this.engine.GestureDetected += Engine_GestureDetected;
        }

        override protected void OnNavigatedTo(NavigationEventArgs e)
        {
            // connect to the nearest Kinemic Band
            engine.ConnectStrongest();
        }

        override protected void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // disconnect our Kinemic Band
            if (band != null) engine.Disconnect(band);
        }

        private void Engine_ConnectionStateChanged(Engine sender, ConnectionStateChangedEventArgs e)
        {
            // remember the current band we interact with
            band = e.Band;
            
            // show a message on screen when connection state changes
            TextBlock.Text = e.Band.Substring(0, 5).ToUpper() + " - Connection state: " + e.State.ToString();
        }

        private void Engine_GestureDetected(Engine sender, GestureDetectedEventArgs e)
        {
            // show a message on screen for each gesture event
            TextBlock.Text = e.Band.Substring(0, 5).ToUpper() + " - Detected gesture: " + e.Gesture.ToString();

            // give haptic feedback for every gesture
            this.engine.Vibrate(e.Band, 300);
        }

    }
}
