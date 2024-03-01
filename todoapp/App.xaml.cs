using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using todoapp.ViewModel;

namespace todoapp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(TodoListStore.DeserializeTodoLists())
            };
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
