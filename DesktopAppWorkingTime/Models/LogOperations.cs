using DesktopAppWorkingTime.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DesktopAppWorkingTime.Models
{
    class LogOperations
    {
        static string logPath = AppDomain.CurrentDomain.BaseDirectory;
        //public static string fileName = $@"{logPath}\cache\log.txt";
        public static string fileName = @"C:\Users\maxim\Desktop\StempelUhr\Zeiten.txt";

        private static string FullDayLogString(Day selectedDay)
        {
            return $"{selectedDay.Date.ToString("dd.MM.yyyy")} | {selectedDay.StartTime.ToString("HH:mm:ss")} - {selectedDay.EndTime.ToString("HH:mm:ss")}";
        }

        private static string CurrentDayLogString(Day selectedDay)
        {
            return $"{selectedDay.Date.ToString("dd.MM.yyyy")} | {selectedDay.StartTime.ToString("HH:mm:ss")} - ";
        }

        public static void RecordStartTime()
        {
            Day newDay = new Day { Date = DateTime.Today, StartTime = DateTime.Now };

            if (IsFirstTimeRecordToday())
            {
                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    writer.Write(CurrentDayLogString(newDay));
                }
            }
            else
            {
                RemoveEndTime();
            }
        }

        public static void RecordEndTime()
        {
            DateTime endTime = DateTime.Now;

            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine(endTime.ToString("HH:mm:ss"));
            }
        }

        public static void RemoveEndTime()
        {
            List<Day> recordedDays = GetRecordedDays();

            Day doubleDay = GetSelectedDay(DateTime.Today);

            recordedDays.RemoveAll(x => x.Date == doubleDay.Date);
            recordedDays = recordedDays.OrderBy(x => x.Date).ToList();

            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                foreach (Day day in recordedDays)
                {
                    writer.WriteLine(FullDayLogString(day));
                }

                writer.Write(CurrentDayLogString(doubleDay));
            }
        }

        public static TimeSpan GetTotatalBalance()
        {
            TimeSpan balance;

            TimeSpan reference = new TimeSpan(GetRecordedDays().Count() * 8, 0, 0);

            TimeSpan actual = new TimeSpan(0, 0, 0);
            foreach (Day day in GetRecordedDays())
            {
                actual += day.Balance;
            }

            balance = actual - reference;
            return new TimeSpan(balance.Hours, balance.Minutes, balance.Seconds);
        }

        public static TimeSpan GetBalanceExcludingToday()
        {
            TimeSpan balance;
            List<Day> recordedDaysWithoutToday = GetRecordedDays();
            recordedDaysWithoutToday.Remove(recordedDaysWithoutToday.Last());

            TimeSpan reference = new TimeSpan(recordedDaysWithoutToday.Count() * 8, 0, 0);

            TimeSpan actual = new TimeSpan(0, 0, 0);
            foreach (Day day in recordedDaysWithoutToday)
            {
                actual += day.Balance;
            }

            balance = actual - reference;
            return new TimeSpan(balance.Hours, balance.Minutes, balance.Seconds);
        }

        public static List<Day> GetRecordedDays()
        {
            List<string> lines = new List<string>();

            try
            {
                lines = File.ReadAllLines(fileName).ToList();
            }
            catch
            {
                return null;
            }

            List<Day> recordedDays = new List<Day>();


            foreach (var line in lines)
            {
                string[] entries = line.Split('|', '-');

                Day selectedDay = new Day();
                selectedDay.Date = Convert.ToDateTime(entries[0]);
                selectedDay.StartTime = Convert.ToDateTime(entries[1]);

                try
                {
                    selectedDay.EndTime = Convert.ToDateTime(entries[2]);
                }
                catch
                {
                    selectedDay.EndTime = DateTime.Now;
                }

                recordedDays.Add(selectedDay);
            }

            return recordedDays;
        }

        public static Day GetSelectedDay(DateTime selectedDate)
        {
            if (GetRecordedDays().Exists(x => x.Date == selectedDate))
            {
                return GetRecordedDays().Find(x => x.Date == selectedDate);
            }
            else
            {
                return new Day();
            }

        }

        public static bool IsFirstTimeRecordToday()
        {
            return !GetRecordedDays().Exists(x => x.Date == DateTime.Today);
        }

        public static void UpdateDay(Day updatedDay)
        {
            List<Day> recordedDays = GetRecordedDays();
            recordedDays.Find(x => x.Date == updatedDay.Date);
            recordedDays.Add(updatedDay);
            recordedDays.OrderBy(x => x.Date);

            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                foreach (Day day in recordedDays)
                {
                    writer.WriteLine();
                }
            }
        }
    }
}
