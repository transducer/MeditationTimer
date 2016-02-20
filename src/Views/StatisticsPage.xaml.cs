using GalaSoft.MvvmLight.Messaging;
using Rooijakkers.MeditationTimer.Messages;
using Rooijakkers.MeditationTimer.ViewModel;

using System;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Rooijakkers.MeditationTimer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StatisticsPage : Page
    {
        private Point _initialPoint; // Point used to store start position so a swipe can be recognized

        public StatisticsPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            Messenger.Default.Register<DisplayDiaryMessage>(this, ReceiveDisplayDiaryMessage);

            SwipingSurface.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            SwipingSurface.ManipulationStarted += SetInitialPosition;
            SwipingSurface.ManipulationCompleted += ToDiaryIfSwipedRight;
        }

        private void ReceiveDisplayDiaryMessage(DisplayDiaryMessage msg)
        {
            if (msg.Display)
            {
                ListViewNoItems.Visibility = Visibility.Collapsed;
                StatisticsListView.Visibility = Visibility.Visible;
                StatisticsListViewHeaders.Visibility = Visibility.Visible;
            }
            else
            {
                ListViewNoItems.Visibility = Visibility.Visible;
                StatisticsListView.Visibility = Visibility.Collapsed;
                StatisticsListViewHeaders.Visibility = Visibility.Collapsed;
            }
        }

        public void SetInitialPosition(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _initialPoint = e.Position;
        }

        public void ToDiaryIfSwipedRight(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var currentPoint = e.Position;
            if (currentPoint.X - _initialPoint.X >= Constants.SWIPING_TRESHOLD)
            {
                NavigateToDiary();
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /* NOTE: The code below is duplicated on all pages. I do not know how to extract it to separate page. */
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
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(StatisticsPage)));
        }

        private async void NavigateToSettings()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(SettingsPage)));
        }
    }
}
