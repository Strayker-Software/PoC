using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace GitGui
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClickHandler(object sender, RoutedEventArgs e)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "git",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                Arguments = "status"
            };

            messageO.Text = processInfo.WorkingDirectory;
            var process = Process.Start(processInfo);

            process!.WaitForExit();

            error.Text = process.StandardError.ReadToEnd();
            message.Text = process.StandardOutput.ReadToEnd();
        }
    }
}