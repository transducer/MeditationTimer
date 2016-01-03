using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Rooijakkers.MeditationTimer
{
    public sealed partial class RootPage : Page
    {
        public RootPage()
        {
            this.InitializeComponent();
        }

        private void ViewTimerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToTimer();
        }

        private void ViewHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToDiary();
        }

        private void ViewSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToSettings();
        }

        private void ViewStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToStatistics();
        }

        private async void NavigateToTimer()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(TimerPage)));
        }

        private async void NavigateToDiary()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(MeditationDiaryPage)));
        }

        private async void NavigateToStatistics()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(MeditationDiaryPage)));
        }

        private async void NavigateToSettings()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(MeditationDiaryPage)));
        }
   }
}
