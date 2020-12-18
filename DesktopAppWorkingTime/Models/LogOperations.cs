﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace DesktopAppWorkingTime.Models
{
    class LogOperations
    {
        //private static string logPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Stempeluhr";
        //private static string fileName = $@"{logPath}\log.txt";
        //static string logPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string fileName = @"C:\Users\maxim\Desktop\StempelUhr\Zeiten.txt";

        private static string FullDayLogString(Day selectedDay)
        {
            return $"{selectedDay.Date.ToString("dd.MM.yyyy")} | {selectedDay.StartTime.ToString("HH:mm:ss")} - {selectedDay.EndTime.ToString("HH:mm:ss")}";
        }
        private static string CurrentDayLogString(Day selectedDay)
        {
            return $"{selectedDay.Date.ToString("dd.MM.yyyy")} | {selectedDay.StartTime.ToString("HH:mm:ss")} - ";
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
            UseStreamWriter(DateTime.Now.ToString("HH:mm:ss"), true, true);
        }

        public static void RemoveEndTime()
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
                        StartTime = Convert.ToDateTime(entries[1]),
                        EndTime = Convert.ToDateTime(entries[2])
                    };

                    recordedDays.Add(selectedDay);
                }
                catch
                {
                    Day selectedDay = new Day
                    {
                        Date = Convert.ToDateTime(entries[0]),
                        StartTime = Convert.ToDateTime(entries[1])
                    };

                    recordedDays.Add(selectedDay);
                }
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
