using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Rooijakkers.MeditationTimer
{
    /// <summary>
    /// Page that is used to have the Splash screen displayed bit longer and pre-load the meditation diary data
    /// </summary>
    public sealed partial class SplashScreenPage : Page
    {
        public SplashScreenPage()
        {
            this.InitializeComponent();

            //Call MainPage from ExtendedSplashScreen after some delay  
            ExtendedSplashScreen();
        }

        async void ExtendedSplashScreen()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        { 
        }
    }
}
