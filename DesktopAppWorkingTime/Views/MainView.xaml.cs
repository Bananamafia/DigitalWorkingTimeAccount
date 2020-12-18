using DesktopAppWorkingTime.Models;
using DesktopAppWorkingTime.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopAppWorkingTime.Views
{
    /// <summary>
    /// Interaktionslogik für MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private MainViewModel _viewModel;

        public MainView()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;

            this.WindowState = WindowState.Minimized;

            base.OnClosing(e);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var txtControl = sender as TextBox;
            txtControl.Dispatcher.BeginInvoke(new Action(() =>
            {
                txtControl.SelectAll();
            }));
        }
    }
}
