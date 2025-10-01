using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _02_TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grid.ItemsSource = Process.GetProcesses();
            //(grid.SelectedItem as Process)
        }

        private void Kill_Click(object sender, RoutedEventArgs e)
        {
            (grid.SelectedItem as Process)?.Kill();
            grid.ItemsSource = Process.GetProcesses();
        }

        private void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            var proc = grid.SelectedItem as Process;
            if (proc != null)
            {
                string info = $"Name: {proc.ProcessName}\n" +
                              $"ID: {proc.Id}\n" +
                              $"Memory: {proc.WorkingSet64 / 1024 / 1024} MB";
                MessageBox.Show(info, "Process Info");
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(nameProc.Text);
            }
            catch
            {
                MessageBox.Show("Не вдалося запустити процес.");
            }
        }
    }

    }
