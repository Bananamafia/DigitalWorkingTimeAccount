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
using System.Windows.Forms;
using System.Drawing;
using System.IO;

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
            this.Hide();
            base.OnClosing(e);

            System.Windows.Forms.NotifyIcon notificationIcon = new System.Windows.Forms.NotifyIcon();
            Stream iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Resources/Icon/time-16.ico")).Stream;
            notificationIcon.Icon = new System.Drawing.Icon(iconStream);
            notificationIcon.Visible = true;

            notificationIcon.ShowBalloonTip(3000, "What's Time", "Programm wird im Hintergrund weiter ausgeführt.", ToolTipIcon.None);

            notificationIcon.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                    notificationIcon.Visible = false;
                };
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var txtControl = sender as System.Windows.Controls.TextBox;
            txtControl.Dispatcher.BeginInvoke(new Action(() =>
            {
                txtControl.SelectAll();
            }));
        }
    }
}
