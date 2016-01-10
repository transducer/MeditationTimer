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
    public sealed partial class SettingsPage : Page
    {
        private Point _initialPoint; // Point used to store start position so a swipe can be recognized

        public SettingsPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            Messenger.Default.Register<DisplayNotificationSoundPickerMessage>(this, ReceiveFiveMinutesBeforeEndCheckBoxValueChangedMessage);

            SwipingSurface.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            SwipingSurface.ManipulationStarted += SetInitialPosition;
            SwipingSurface.ManipulationCompleted += ToDiaryIfSwipedLeft;
            SwipingSurface.ManipulationCompleted += ToStatisticsIfSwipedRight;
        }

        public void SetInitialPosition(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _initialPoint = e.Position;
        }

        private void ReceiveFiveMinutesBeforeEndCheckBoxValueChangedMessage(DisplayNotificationSoundPickerMessage msg) 
        {
            if (msg.Display)
            {
                NoticationsComboBox.Visibility = Visibility.Visible;
                NotificationSoundTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                NoticationsComboBox.Visibility = Visibility.Collapsed;
                NotificationSoundTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        public void ToStatisticsIfSwipedRight(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var currentPoint = e.Position;
            if (currentPoint.X - _initialPoint.X >= Constants.SWIPING_TRESHOLD)
            {
                NavigateToStatistics();
            }
        }

        public void ToDiaryIfSwipedLeft(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var currentPoint = e.Position;
            if (_initialPoint.X - currentPoint.X >= Constants.SWIPING_TRESHOLD)
            {
                NavigateToDiary();
            }
        }

        private async void NavigateToStatistics()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(TimerPage)));
        }

        private async void NavigateToDiary()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(MeditationDiaryPage)));
        }

        private async void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Return to meditation timer when settings are accepted. Command on view model can handle other stuff.
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(TimerPage)));
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Reset value in settings so that the saved value are displayed on the page.
            ViewModel.SetValuesToSettings();
        }

        public SettingsViewModel ViewModel
        {
            get
            {
                return this.DataContext as SettingsViewModel;
            }
        }
    }
}