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
            // TODO: Retrieve list of meditations entries here while waiting
            await Task.Delay(TimeSpan.FromSeconds(3));
            Frame.Navigate(typeof(MainPage));
        }
   }
}
