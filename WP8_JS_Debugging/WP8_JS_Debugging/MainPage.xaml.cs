using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace WP8_JS_Debugging
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Url of Home page
        private string MainUri = "/Html/index.html";

        // Debugging source
        ObservableCollection<ScriptNotify> notifications = new ObservableCollection<ScriptNotify>();
        DateTime startTime;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            startTime = DateTime.Now;
            if (Debugger.IsAttached)
            {
                DebugPanel.Visibility = Visibility.Visible;

                ApplicationBar = new ApplicationBar();
                ApplicationBar.Mode = ApplicationBarMode.Minimized;
                ApplicationBar.Opacity = 1.0;
                ApplicationBar.IsVisible = true;
                ApplicationBar.IsMenuEnabled = true;

                // Adding a menu item
                ApplicationBarMenuItem myMenuItem = new ApplicationBarMenuItem();
                myMenuItem.Text = "open debugger";
                myMenuItem.Click += toggleDebugger;
                ApplicationBar.MenuItems.Add(myMenuItem);
                // Adding a menu item
                ApplicationBarMenuItem minDebugger = new ApplicationBarMenuItem();
                minDebugger.Text = "minimize debugger";
                minDebugger.Click += minDebug;
                ApplicationBar.MenuItems.Add(minDebugger);
            }
        }      
        
        void toggleDebugger(object sender, EventArgs e)
        {
            if (DebugPanel.ActualHeight > 1)
            {
                HideDebuggerUI.Begin();
                ((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "open debugger";
            }
            else
            {
                ShowDebuggerUI.Begin();
                ((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "close debugger";
            }
        }

        void minDebug(object sender, EventArgs e)
        {
            MinDebuggerUI.Begin();
            ((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "close debugger";
        }

        private void ShowDebugger(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowDebuggerUI.Begin();
        }

        private void startDebugger(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            ShowDebuggerUI.Begin();
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            // Add your URL here
            Browser.Navigate(new Uri(MainUri, UriKind.Relative));
            if(Debugger.IsAttached)
                DebugPanel.ItemsSource = notifications;
        }

        // Navigates to the initial "home" page.
        private void HomeMenuItem_Click(object sender, EventArgs e)
        {
            Browser.Navigate(new Uri(MainUri, UriKind.Relative));
        }

        // Handle navigation failures.
        private void Browser_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            MessageBox.Show("Navigation to this page failed, check your internet connection");
        }

        private void Browser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                ScriptNotify newValue = new ScriptNotify()
                {
                    Value = e.Value,
                    Time = DateTime.Now,
                    FormattedTime = "at " + (DateTime.Now.Subtract(startTime)).TotalMilliseconds.ToString("0,000") + " milliseconds"
                };
                notifications.Add(newValue);

                DebugPanel.ScrollTo(newValue);
            }
        }
    }

    public class ScriptNotify
    {
        public ScriptNotify(){}

        public string Value {get; set;}
        public DateTime Time {get; set;}
        public string FormattedTime { get; set; }
    }
}
