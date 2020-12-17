using DesktopAppWorkingTime.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DesktopAppWorkingTime.ViewModels.Commands
{
    class UpdateTimesCommand : ICommand
    {
        private Day SelectedDay { get; set; }

        public UpdateTimesCommand(Day selectedDay)
        {
            SelectedDay = selectedDay;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //throw new NotImplementedException();
            return true;
        }

        public void Execute(object parameter)
        {
            LogOperations.UpdateDay(SelectedDay);
        }
    }
}
