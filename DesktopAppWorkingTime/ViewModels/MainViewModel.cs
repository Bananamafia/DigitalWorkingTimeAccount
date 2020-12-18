using System;
using System.Collections.Generic;
using System.Text;
using DesktopAppWorkingTime.Models;
using DesktopAppWorkingTime.ViewModels.Commands;
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;
using System.Windows;

namespace DesktopAppWorkingTime.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            string logPath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Stempeluhr";
            Directory.CreateDirectory(logPath);

            try
            {
                LogOperations.RecordStartTime();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

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
            _currentBalance = LogOperations.GetBalanceExcludingToday();

            try
            {                
                Day selectedDay = LogOperations.GetSelectedDay(CurrentDate);

                _startTimeHour = selectedDay.StartTime.ToString("HH");
                _startTimeMin = selectedDay.StartTime.ToString("mm");

                _lunchInMin = selectedDay.LunchInMin.Minutes;

                _endTimeHour = selectedDay.EndTime.ToString("HH");
                _endTimeMin = selectedDay.EndTime.ToString("mm");
            }
            catch (NullReferenceException)
            {
                _startTimeHour = "N/A";
                _startTimeMin = "N/A";

                _lunchInMin = 45;

                _endTimeHour = "N/A";
                _endTimeMin = "N/A";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        //---Properties---
        public string YesterdaysDate { get; set; } = (DateTime.Today - new TimeSpan(1, 0, 0, 0)).ToString("dd.MM.yyy");
        private DateTime _currentDate = DateTime.Today;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                SetUpTextBoxes();

                OnPropertyChanged("CurrentDate");
                OnPropertyChanged("CurrentBalance");

                OnPropertyChanged("StartTimeHour");
                OnPropertyChanged("StartTimeMin");

                OnPropertyChanged("LunchInMin");

                OnPropertyChanged("EndTimeHour");
                OnPropertyChanged("EndTimeMin");

                OnPropertyChanged("UpdateTimesCommand");
            }
        }

        private TimeSpan _currentBalance;
        public TimeSpan CurrentBalance
        {
            get
            {
                return _currentBalance;
            }
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
                OnPropertyChanged("UpdateTimesCommand");
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
                OnPropertyChanged("UpdateTimesCommand");
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
                OnPropertyChanged("UpdateTimesCommand");
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
                OnPropertyChanged("UpdateTimesCommand");
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
                OnPropertyChanged("UpdateTimesCommand");
            }
        }

        //---Commands---
        private UpdateTimesCommand _updateTimesCommand;
        public UpdateTimesCommand UpdateTimesCommand
        {
            get
            {
                DateTime StartTime = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, Convert.ToInt32(StartTimeHour), Convert.ToInt32(StartTimeMin), 0);
                DateTime EndTime = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, Convert.ToInt32(EndTimeHour), Convert.ToInt32(EndTimeMin), 0);
                TimeSpan LunchTime = new TimeSpan(0, LunchInMin, 0);

                _updateTimesCommand = new UpdateTimesCommand(new Day { Date = CurrentDate, StartTime = StartTime, EndTime = EndTime, LunchInMin = LunchTime });

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
