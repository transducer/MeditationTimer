/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Rooijakkers.MeditationTimer"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Rooijakkers.MeditationTimer.Data;
using Rooijakkers.MeditationTimer.Data.Contracts;

namespace Rooijakkers.MeditationTimer.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IMeditationDiaryRepository, MeditationDiaryRepository>();
            SimpleIoc.Default.Register<TimerViewModel>();
            SimpleIoc.Default.Register<DiaryViewModel>();
        }

        public TimerViewModel Timer => ServiceLocator.Current.GetInstance<TimerViewModel>();

        public DiaryViewModel Diary => ServiceLocator.Current.GetInstance<DiaryViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}