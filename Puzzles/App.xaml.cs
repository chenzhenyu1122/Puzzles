using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Puzzles
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain CurrentDomain = AppDomain.CurrentDomain;
            CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Handler);
        }
        static void Handler(object Sender, UnhandledExceptionEventArgs Args)
        {
            Exception ex = (Exception)Args.ExceptionObject;
            MessageBox.Show(ex.Message);
        }
    }
}
