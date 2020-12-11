using System;
using System.IO;
using System.Linq;

namespace ConsoleApplicationWorkingTime
{
    class Program
    {
        static void Main(string[] args)
        {
            RecordEndTime();
        }

        static string filename = @"C:\Users\maxim\Desktop\StempelUhr\Zeiten.txt";


        static void RecordStartTime()
        {
            DateTime startTime = DateTime.Now;

            using (StreamWriter writer = new StreamWriter(filename, true))
            {
                writer.WriteLine("Kommen:");
                writer.WriteLine(startTime);
            }

            Console.WriteLine("Startzeit wurde erfasst.");
        }

        static void RecordEndTime(int breakTime = 45)
        {
            DateTime endTime = DateTime.Now;
            string workingTime = (endTime - GetLastRecordedTime() - GetBreak(breakTime)).ToString();

            using (StreamWriter writer = new StreamWriter(filename, true))
            {
                writer.WriteLine("Gehen: ");
                writer.WriteLine(endTime);

                writer.WriteLine("Tages-Saldo:");
                writer.WriteLine(workingTime);
            }

            Console.WriteLine("Endzeit wurde erfasst.");
        }

        static DateTime GetLastRecordedTime()
        {
            return Convert.ToDateTime(File.ReadLines(filename).Last());
        }

        static TimeSpan GetBreak(int timeInMin = 45)
        {
            return new TimeSpan(0, timeInMin, 0);
        }
    }
}
