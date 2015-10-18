using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Messaging;
using Rooijakkers.MeditationTimer.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Rooijakkers.MeditationTimer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Point _initialPoint;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            Messenger.Default.Register<PlayMessage>(this, ReceivePlayMessage);
            Messenger.Default.Register<StartTimerMessage>(this, ReceiveStartTimerMessage);
            Messenger.Default.Register<StopTimerMessage>(this, ReceiveStopTimerMessage);

            SwipingSurface.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            SwipingSurface.ManipulationStarted += SetInitialPosition;
            SwipingSurface.ManipulationCompleted += ToDiaryIfSwipedLeft;
        }

        public void SetInitialPosition(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _initialPoint = e.Position;
        }

        public void ToDiaryIfSwipedLeft(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var currentPoint = e.Position;
            if (_initialPoint.X - currentPoint.X >= Constants.SwipingTreshold)
            {
                NavigateToDiary();
            }
        }

        private void ReceivePlayMessage(PlayMessage msg)
        {
            BurmeseGongMediaElement.Play();
        }

        private void ReceiveStartTimerMessage(StartTimerMessage msg)
        {
            StartTimerButton.Visibility = Visibility.Collapsed;
            StopTimerButton.Visibility = Visibility.Visible;
            AddFiveMinutesButton.IsEnabled = false;
            ResetInitialTimeButton.IsEnabled = false;
        }

        private void ReceiveStopTimerMessage(StopTimerMessage msg)
        {
            StartTimerButton.Visibility = Visibility.Visible;
            StopTimerButton.Visibility = Visibility.Collapsed;
            AddFiveMinutesButton.IsEnabled = true;
            ResetInitialTimeButton.IsEnabled = true;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void ViewHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToDiary();
        }

        private void NavigateToDiary()
        {
            Frame.Navigate(typeof(MeditationDiaryPage));
        }
    }
}
