using System;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Rooijakkers.MeditationTimer.Data;
using Rooijakkers.MeditationTimer.Data.Contracts;
using Rooijakkers.MeditationTimer.Model;

namespace Rooijakkers.MeditationTimer.ViewModel
{
    public class MeditationDiaryViewModel : ViewModelBase
    {
        private readonly IMeditationDiaryRepository _repository;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MeditationDiaryViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                _repository = new MeditationDiaryRepository();
            }
        }
    }
}