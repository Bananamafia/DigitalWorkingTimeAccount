using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApplicationWorkingTime
{
    class Program
    {
        static void Main(string[] args)
        {
            RecordStartTime();
        }

        static string fileName = @"C:\Users\maxim\Desktop\StempelUhr\Zeiten.txt";


        static void RecordStartTime()
        {
            DateTime startTime = DateTime.Now;

            if (IsFirstTimeRecordToday())
            {
                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    writer.Write($"{startTime.ToString("dd.MM.yyyy")} | ");
                    writer.Write($"{startTime.ToString("HH:mm:ss")} - ");
                }

                Console.WriteLine("Startzeit wurde erfasst.");
            }
            else
            {
                Console.WriteLine("Du warst heute schon mal online. Willkommen zurück!");
                RemoveEndTime();
            }
        }

        static void RecordEndTime()
        {
            DateTime endTime = DateTime.Now;

            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine(endTime.ToString("HH:mm:ss"));
            }

            Console.WriteLine("Endzeit wurde erfasst.");
        }

        static void RemoveEndTime()
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

        static List<Day> GetRecordedDays()
        {
            List<string> lines = File.ReadAllLines(fileName).ToList();
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

        static bool IsFirstTimeRecordToday()
        {
            return !GetRecordedDays().Exists(x => x.Date == DateTime.Today);
        }
    }
}
