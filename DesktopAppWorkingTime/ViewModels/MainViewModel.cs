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

            if(!LogOperations.GetRecordedDays().Exists(x => x.Date == DateTime.Today))
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

        private static DateTime _currentDate = DateTime.Today;
        public static DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                //OnPropertyChanged("CurrentDate");
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

        private string _startTimeHour = LogOperations.GetSelectedDay(CurrentDate).StartTime.ToString("HH");
        public string StartTimeHour
        {
            get { return _startTimeHour; }
            set
            {
                _startTimeHour = value;
                OnPropertyChanged("StartTimeHour");
                OnPropertyChanged("CurrentBalance");
            }
        }

        private string _startTimeMin = LogOperations.GetSelectedDay(CurrentDate).StartTime.ToString("mm");
        public string StartTimeMin
        {
            get { return _startTimeMin; }
            set
            {
                _startTimeMin = value;
                OnPropertyChanged("StartTimeMin");
                OnPropertyChanged("CurrentBalance");
            }
        }

        private int _lunchInMin = LogOperations.GetSelectedDay(CurrentDate).LunchInMin.Minutes;
        public int LunchInMin
        {
            get { return _lunchInMin; }
            set
            {
                _lunchInMin = value;
                OnPropertyChanged("LunchInMin");
                OnPropertyChanged("CurrentBalance");
            }
        }

        private string _endTimeHour = LogOperations.GetSelectedDay(CurrentDate).EndTime.ToString("HH");
        public string EndTimeHour
        {
            get { return _endTimeHour; }
            set
            {
                _endTimeHour = value;
                OnPropertyChanged("EndTimeHour");
                OnPropertyChanged("CurrentBalance");
            }
        }

        private string _endTimeMin = LogOperations.GetSelectedDay(CurrentDate).EndTime.ToString("mm");
        public string EndTimeMin
        {
            get { return _endTimeMin; }
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
