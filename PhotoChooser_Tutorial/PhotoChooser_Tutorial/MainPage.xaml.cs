using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace PhotoChooser_Tutorial
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

        

        //////////////////////////////////////////////////////////////
        // The C# code for integrating the camera into an HTML app
        //   Step 1: Catch Javascript window.external.notify signal 
        //   Step 2: Launch the PhotoChooser, get the photo back
        //   Step 3: Send the photo as a string to the HTML app
        /////////////////////////////////////////////////////////////
        
        // Establish PhotoChooserTask variable
        PhotoChooserTask _photoChooser;

        // Step 1: Catch Javascript signal
        private void Catch_Script_Notification(object sender, NotifyEventArgs e)
        {
            if (e.Value.StartsWith("LaunchPhotoChooser"))
            {
                // Step 2: Launch the PhotoChooser
                _photoChooser = new PhotoChooserTask();
                _photoChooser.Completed += _photoChooser_Completed;
                // allow the user to either choose a saved image or take a new one
                _photoChooser.ShowCamera = true;
                _photoChooser.PixelHeight = 260;
                _photoChooser.PixelWidth = 200;
                _photoChooser.Show();
            }
            else
            {
                // used for debuggin
                TextBlock alertBox = new TextBlock();
                alertBox.Text = e.Value;
                notifyAlert.Children.Add(alertBox);
            }
        }

        void _photoChooser_Completed(object sender, PhotoResult e)
        {
            // Step 3: Send the photo back to the HTML app as a string
            byte[] photoArray = new byte[e.ChosenPhoto.Length];
            e.ChosenPhoto.Position = 0;
            e.ChosenPhoto.Read(photoArray, 0, photoArray.Length);
            
            // We need convert this byte array into a string because we can only 
            //   pass strings into the WebBrowser control
            string photoString = Convert.ToBase64String(photoArray);
            
            // switch to the UI thread
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // run the setPhotoWithDimensions function and pass in the byte array with
                //  the "data:image/png;base64" header
                string[] parameters = new string[] { "data:image/png;base64," + photoString, "260", "200" };
                
                Browser.InvokeScript("setPhotoWithDimensions", parameters);
            });
        }
    }
}