using System;
using System.Collections.Generic;
using System.Text;
using DesktopAppWorkingTime.Models;
using DesktopAppWorkingTime.ViewModels.Commands;
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;

namespace DesktopAppWorkingTime.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            //Directory.CreateDirectory($@"{AppDomain.CurrentDomain.BaseDirectory}\cache");

            //if (!File.Exists(LogOperations.fileName))
            //{
            //    File.Create(LogOperations.fileName);
            //}

            LogOperations.RecordStartTime();

            SystemEvents.SessionEnding += SystemEvents_SessionEnding;

            SetUpTextBoxes();
        }

        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            LogOperations.RecordEndTime();
            System.Windows.Application.Current.Shutdown();
        }

        private void SetUpTextBoxes()
        {
            _startTimeHour = LogOperations.GetSelectedDay(CurrentDate).StartTime.ToString("HH");
            _startTimeMin = LogOperations.GetSelectedDay(CurrentDate).StartTime.ToString("mm");
            _lunchInMin = LogOperations.GetSelectedDay(CurrentDate).LunchInMin.Minutes;
            _endTimeHour = LogOperations.GetSelectedDay(CurrentDate).EndTime.ToString("HH");
            _endTimeMin = LogOperations.GetSelectedDay(CurrentDate).EndTime.ToString("mm");
        }

        //---Properties---
        private DateTime _currentDate = DateTime.Today;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                SetUpTextBoxes();

                OnPropertyChanged("CurrentDate");
                OnPropertyChanged("StartTimeHour");
                OnPropertyChanged("StartTimeMin");
                OnPropertyChanged("LunchInMin");
                OnPropertyChanged("EndTimeHour");
                OnPropertyChanged("EndTimeMin");
                OnPropertyChanged("UpdateTimesCommand");
            }
        }

        private TimeSpan _currentBalance = LogOperations.GetBalanceExcludingToday();
        public TimeSpan CurrentBalance
        {
            get { return _currentBalance; }
            set
            {
                _currentBalance = value;
                OnPropertyChanged("CurrentBalance");
            }
        }

        private string _startTimeHour;
        public string StartTimeHour
        {
            get
            {
                return _startTimeHour;
            }
            set
            {
                _startTimeHour = value;
                OnPropertyChanged("StartTimeHour");
                OnPropertyChanged("CurrentBalance");
            }
        }

        private string _startTimeMin;
        public string StartTimeMin
        {
            get
            {
                return _startTimeMin;
            }
            set
            {
                _startTimeMin = value;
                OnPropertyChanged("StartTimeMin");
                OnPropertyChanged("CurrentBalance");
            }
        }

        private int _lunchInMin;
        public int LunchInMin
        {
            get
            {
                return _lunchInMin;
            }
            set
            {
                _lunchInMin = value;
                OnPropertyChanged("LunchInMin");
                OnPropertyChanged("CurrentBalance");
            }
        }

        private string _endTimeHour;
        public string EndTimeHour
        {
            get
            {
                return _endTimeHour;
            }
            set
            {
                _endTimeHour = value;
                OnPropertyChanged("EndTimeHour");
                OnPropertyChanged("CurrentBalance");
            }
        }

        private string _endTimeMin;
        public string EndTimeMin
        {
            get
            {
                return _endTimeMin;
            }
            set
            {
                _endTimeMin = value;
                OnPropertyChanged("EndTimeMin");
                OnPropertyChanged("CurrentBalance");
            }
        }

        //---Commands---
        private UpdateTimesCommand _updateTimesCommand;
        public UpdateTimesCommand UpdateTimesCommand
        {
            get
            {
                _updateTimesCommand = new UpdateTimesCommand(LogOperations.GetSelectedDay(CurrentDate));
                return _updateTimesCommand;
            }
        }


        //---INotifyPropertyChanged---
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
