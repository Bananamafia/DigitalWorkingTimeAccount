using System;
using System.Collections.Generic;
using System.Text;
using DesktopAppWorkingTime.Models;
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

            if (!LogOperations.GetRecordedDays().Exists(x => x.Date == DateTime.Today))
            {
                LogOperations.RecordStartTime();
            }

            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
        }

        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            LogOperations.RecordEndTime();
            System.Windows.Application.Current.Shutdown();
        }

        private DateTime _currentDate = DateTime.Today;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                OnPropertyChanged("CurrentDate");
                OnPropertyChanged("StartTimeHour");
                OnPropertyChanged("StartTimeMin");
                OnPropertyChanged("LunchInMin");
                OnPropertyChanged("EndTimeHour");
                OnPropertyChanged("EndTimeMin");
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
                _startTimeHour = LogOperations.GetSelectedDay(CurrentDate).StartTime.ToString("HH");
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
                _startTimeMin = LogOperations.GetSelectedDay(CurrentDate).StartTime.ToString("mm");
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
                _lunchInMin = LogOperations.GetSelectedDay(CurrentDate).LunchInMin.Minutes;
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
                _endTimeHour = LogOperations.GetSelectedDay(CurrentDate).EndTime.ToString("HH");
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
                _endTimeMin = LogOperations.GetSelectedDay(CurrentDate).EndTime.ToString("mm");
                return _endTimeMin;
            }
            set
            {
                _startTimeMin = value;
                OnPropertyChanged("EndTimeMin");
                OnPropertyChanged("CurrentBalance");
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
