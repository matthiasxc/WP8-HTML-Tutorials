using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;

namespace Yeti_Test
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Url of Home page
        private string MainUri = "/Html/index.html";

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            // Add your URL here
            Browser.Navigate(new Uri(MainUri, UriKind.Relative));
            Browser.ScriptNotify += HTML_Script_Launched;
        }

        // Navigates back in the web browser's navigation stack, not the applications.
        private void BackApplicationBar_Click(object sender, EventArgs e)
        {
            Browser.GoBack();
        }

        // Navigates forward in the web browser's navigation stack, not the applications.
        private void ForwardApplicationBar_Click(object sender, EventArgs e)
        {
            Browser.GoForward();
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

        private void HTML_Script_Launched(object sender, NotifyEventArgs e)
        {
            MessageBox.Show(e.Value);
        }

        private void Browser_Navigated_1(object sender, NavigationEventArgs e)
        {
            // Run Javascript by calling a function
            //Browser.InvokeScript("changeTitle");

            // Run Javascript by calling a function with parameters
            //Browser.InvokeScript("changeTitle", new string[]{"change using parameter"});

            // Run Javascript by adding raw Javascript as a string
            Browser.InvokeScript("eval", new string[] { "document.getElementById('dynamicTitle').innerHTML = 'injected javascript';" });
        }

    }
}