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
            Console.WriteLine(IsFirstTimeRecordToday());

            //foreach (var day in GetRecordedDays())
            //{
            //    Console.WriteLine($"{day.Date.ToString("dd.MM.yyy")} | {day.StartTime.ToString("hh.mm.ss")} - {day.EndTime.ToString("hh.mm.ss")} | Saldo: {day.Balance}");
            //}
        }

        static string fileName = @"C:\Users\maxim\Desktop\StempelUhr\Zeiten.txt";


        static void RecordStartTime()
        {
            DateTime startTime = DateTime.Now;

            //check if there was allready a starttime today if so, delete endtime of this set

            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.Write($"{startTime.ToString("dd.MM.yyyy")} | ");
                writer.Write($"{startTime.ToString("hh:mm:ss")} - ");
            }

            Console.WriteLine("Startzeit wurde erfasst.");
        }

        static void RecordEndTime(int breakTime = 45)
        {
            DateTime endTime = DateTime.Now;

            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine(endTime.ToString("hh:mm:ss"));
            }

            Console.WriteLine("Endzeit wurde erfasst.");
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
