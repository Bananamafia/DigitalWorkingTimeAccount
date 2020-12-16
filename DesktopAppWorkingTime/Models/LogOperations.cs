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

        public static void RecordStartTime()
        {
            DateTime startTime = DateTime.Now;

            if (IsFirstTimeRecordToday())
            {
                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    writer.Write($"{startTime.ToString("dd.MM.yyyy")} | ");
                    writer.Write($"{startTime.ToString("HH:mm:ss")} - ");
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

            Day doubleDay = recordedDays.Find(x => x.Date == DateTime.Today);
            recordedDays.Remove(doubleDay);

            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                foreach (Day day in recordedDays)
                {
                    writer.WriteLine($"{day.Date.ToString("dd.MM.yyyy")} | {day.StartTime.ToString("HH:mm:ss")} - {day.EndTime.ToString("HH:mm:ss")}");
                }

                writer.Write($"{doubleDay.Date.ToString("dd.MM.yyyy")} | {doubleDay.StartTime.ToString("HH:mm:ss")} - ");
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

        public static bool IsFirstTimeRecordToday()
        {
            return !GetRecordedDays().Exists(x => x.Date == DateTime.Today);
        }
    }
}
