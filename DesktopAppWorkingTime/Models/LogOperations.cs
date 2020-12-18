using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace DesktopAppWorkingTime.Models
{
    class LogOperations
    {
        private static string logPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Stempeluhr";
        private static string fileName = $@"{logPath}\log.txt";
        //static string logPath = AppDomain.CurrentDomain.BaseDirectory;
        //private static string fileName = @"C:\Users\maxim\Desktop\StempelUhr\Zeiten.txt";

        //---Helpers---
        private static string FullDayLogString(Day selectedDay)
        {
            return $"{selectedDay.Date.ToString("dd.MM.yyyy")} | {selectedDay.LunchInMin.Minutes} | {selectedDay.StartTime.ToString("HH:mm:ss")} - {selectedDay.EndTime.ToString("HH:mm:ss")}";
        }
        private static string CurrentDayLogString(Day selectedDay)
        {
            return $"{selectedDay.Date.ToString("dd.MM.yyyy")} | {selectedDay.LunchInMin.Minutes} | {selectedDay.StartTime.ToString("HH:mm:ss")}";
        }
        private static bool IsFirstTimeRecordToday()
        {
            try
            {
                return !GetRecordedDays().Exists(x => x.Date == DateTime.Today);
            }
            catch (System.IO.IOException)
            {
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        private static void UseStreamWriter(string text, bool newLine, bool append)
        {
            using (StreamWriter writer = new StreamWriter(fileName, append))
            {
                if (newLine)
                {
                    writer.WriteLine(text);
                }
                else
                {
                    writer.Write(text);
                }
            }
        }


        public static List<Day> GetRecordedDays()
        {
            List<string> lines = File.ReadAllLines(fileName).ToList();
            List<Day> recordedDays = new List<Day>();

            foreach (string line in lines)
            {
                string[] entries = line.Split('|', '-');

                try
                {
                    Day selectedDay = new Day
                    {
                        Date = Convert.ToDateTime(entries[0]),
                        StartTime = Convert.ToDateTime(entries[2]),
                        LunchInMin = new TimeSpan(0, Convert.ToInt32(entries[1]), 0),
                        EndTime = Convert.ToDateTime(entries[3])
                    };

                    recordedDays.Add(selectedDay);
                }
                catch (IndexOutOfRangeException)
                {
                    Day selectedDay = new Day
                    {
                        Date = Convert.ToDateTime(entries[0]),
                        StartTime = Convert.ToDateTime(entries[2]),
                        LunchInMin = new TimeSpan(0, Convert.ToInt32(entries[1]), 0)
                    };

                    recordedDays.Add(selectedDay);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            return recordedDays;
        }
        public static Day GetSelectedDay(DateTime selectedDate)
        {
            return GetRecordedDays().Find(x => x.Date == selectedDate);
        }

        public static void RecordStartTime()
        {
            Day newDay = new Day { Date = DateTime.Today, StartTime = DateTime.Now };

            if (IsFirstTimeRecordToday())
            {
                UseStreamWriter(CurrentDayLogString(newDay), false, true);
            }
            else
            {
                RemoveEndTime();
            }
        }

        public static void RecordEndTime()
        {
            UseStreamWriter($" - {DateTime.Now.ToString("HH:mm:ss")}", true, true);
        }

        private static void RemoveEndTime()
        {
            List<Day> recordedDays = GetRecordedDays();

            Day doubleDay = GetSelectedDay(DateTime.Today);

            recordedDays.RemoveAll(x => x.Date == doubleDay.Date);
            recordedDays = recordedDays.OrderBy(x => x.Date).ToList();

            File.Delete(fileName);

            foreach (Day day in recordedDays)
            {
                UseStreamWriter(FullDayLogString(day), true, true);
            }
            UseStreamWriter(CurrentDayLogString(doubleDay), false, true);
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

        public static void UpdateDay(Day updatedDay)
        {
            List<Day> recordedDays = GetRecordedDays();
            Day currentDay = GetSelectedDay(DateTime.Today);
            recordedDays.RemoveAll(x => x.Date == updatedDay.Date || x.Date == currentDay.Date);

            if (!(updatedDay.Date == DateTime.Today))
            {
                recordedDays.Add(updatedDay);
            }
            else
            {
                currentDay = updatedDay;
            }

            recordedDays = recordedDays.OrderBy(x => x.Date).ToList();

            File.Delete(fileName);
            foreach (Day day in recordedDays)
            {
                UseStreamWriter(FullDayLogString(day), true, true);
            }
            UseStreamWriter(CurrentDayLogString(currentDay), false, true);
        }
    }
}
