using System;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Rooijakkers.MeditationTimer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MeditationDiaryPage : Page
    {
        private Point _initialPoint; // Point used to store start position so a swipe can be recognized

        public MeditationDiaryPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            SwipingSurface.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            SwipingSurface.ManipulationStarted += SetInitialPosition;
            SwipingSurface.ManipulationCompleted += ToDiaryIfSwipedRight;
        }

        public void SetInitialPosition(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _initialPoint = e.Position;
        }

        public void ToDiaryIfSwipedRight(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var currentPoint = e.Position;
            if (currentPoint.X - _initialPoint.X >= Constants.SwipingTreshold)
            {
                NavigateToMain();
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Display friendly message to user when diary is empty.

            var emptyDiary = MeditationDiaryListView.Items.Count == 0;

            if (emptyDiary)
            {
                ListViewNoItems.Visibility = Visibility.Visible;
                MeditationDiaryListView.Visibility = Visibility.Collapsed;
                MeditationDiaryListViewHeaders.Visibility = Visibility.Collapsed;
            }
            else
            {
                ListViewNoItems.Visibility = Visibility.Collapsed;
                MeditationDiaryListView.Visibility = Visibility.Visible;
                MeditationDiaryListViewHeaders.Visibility = Visibility.Visible;
            }
        }

        private void NavigateToMain()
        {
            Frame.Navigate(typeof(TimerPage));
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
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(MeditationDiaryPage)));
        }

        private async void NavigateToSettings()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(MeditationDiaryPage)));
        }
    }
}
