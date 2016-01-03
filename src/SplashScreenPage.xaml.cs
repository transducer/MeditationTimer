using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

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

            // Call MainPage from ExtendedSplashScreen after some delay  
            ExtendedSplashScreen();
        }

        private async void ExtendedSplashScreen()
        {
#if DEBUG
            // Pretend like we are doing something for 1 second
            await Task.Delay(TimeSpan.FromSeconds(1));
#else
            // Pretend like we are doing something for 3 seconds
            await Task.Delay(TimeSpan.FromSeconds(3));
#endif
            Frame.Navigate(typeof(TimerPage));
        }
   }
}
