using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _07_CopyFileManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel model = new ViewModel();
        public MainWindow()
        {
            InitializeComponent();
            model.Source = sourceTb.Text = @"D:\My Documents\Temp\Temp1.zip";
            model.Destination = DestTb.Text = @"D:\My Documents\Temp2";
            model.Progress = 0;
            this.DataContext = model;

        }

        private void SourceBtn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
               
                model.Source = sourceTb.Text = fileDialog.FileName;
            }
        }

        private void DestBtn(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                model.Destination = DestTb.Text = dialog.FileName;
            }
        }

        private async void CopyBtn(object sender, RoutedEventArgs e)
        {
            
            string filename = Path.GetFileName(model.Source);
            string destFilename = Path.Combine(model.Destination, filename);
            
            CopyProcessecInfo info = new CopyProcessecInfo()
            {
                Filename = filename,
                Percentage = 0
            };

            model.AddProcess(info);
            await CopyFileAsync(model.Source, destFilename, info);
            MessageBox.Show("Completed!!!");

        }

        private Task CopyFileAsync(string src, string dest, CopyProcessecInfo info)
        {
            return Task.Run(() =>
            {
                
                using FileStream srcStream = new FileStream(src, FileMode.Open, FileAccess.Read);
                using FileStream desStream = new FileStream(dest, FileMode.Create, FileAccess.Write);
                byte[] buffer = new byte[1024 * 8];
                int bytes = 0;
                do
                {
                    bytes = srcStream.Read(buffer, 0, buffer.Length);
                    desStream.Write(buffer, 0, bytes);

                    float percentage = desStream.Length / (srcStream.Length / 100);
                       
                    model.Progress = percentage;
                    info.Percentage = percentage;


                } while (bytes > 0);


            });

        }
    }
    [AddINotifyPropertyChangedInterface]
    class ViewModel
    {
        private ObservableCollection<CopyProcessecInfo> processes;
        public string Source { get; set; }
        public string Destination { get; set; }
        public float Progress { get; set; }
        public bool IsWaiting => Progress == 0;
        public IEnumerable<CopyProcessecInfo> Processes => processes;//get - readonly
        public ViewModel()
        {
            processes = [];
        }
        public void AddProcess(CopyProcessecInfo info)
        {
            processes.Add(info);
        }
    }
    [AddINotifyPropertyChangedInterface]
    class CopyProcessecInfo
    {
        public string Filename { get; set; }
        public float Percentage { get; set; }
    }
}