using System;
using System.Collections.Generic;
using System.Text;
using DesktopAppWorkingTime.Models;
using Microsoft.Win32;
using System.IO;

namespace DesktopAppWorkingTime.ViewModels
{
    class MainViewModel
    {
        public MainViewModel()
        {
            //Directory.CreateDirectory($@"{AppDomain.CurrentDomain.BaseDirectory}\cache");

            //if (!File.Exists(LogOperations.fileName))
            //{
            //    File.Create(LogOperations.fileName);
            //}

            LogOperations.RecordStartTime();
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
        }

        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            LogOperations.RecordEndTime();
            System.Windows.Application.Current.Shutdown();
        }
    }
}
