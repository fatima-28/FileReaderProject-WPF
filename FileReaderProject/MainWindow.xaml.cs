
using FileReaderProject.Helpers;
using FileReaderProject.Models;
using FileReaderProject.Services;
using FileReaderProject.Services.Interfaces;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace FileReaderProject;


public partial class MainWindow : Window
{
    private DirectoryWatcher _watcher;

    public MainWindow()
    {
        InitializeComponent();

        var appSettings = ((App)Application.Current).Configuration.GetSection("AppSettings");

        string inputDirectory = appSettings["InputDirectory"];

        int pollingInterval = int.Parse(appSettings["PollingIntervalSeconds"]);

        _watcher = new DirectoryWatcher();

        _watcher.FileChanged += OnFileChanged;

        _watcher.StartWatching(inputDirectory, pollingInterval);

        LoadFiles(inputDirectory);
    }

    private void OnFileChanged(string filePath)
    {
        Dispatcher.Invoke(() => LoadFiles(_watcher.DirectoryPath));
    }

    private void LoadFiles(string directoryPath)
    {
        FileListBox.Items.Clear();

        var files = Directory.GetFiles(directoryPath);

        foreach (var file in files)
        {
            var fileItem = new ListBoxItem
            {
                Content = Path.GetFileName(file),
                Tag = file
            };

            var eyeIcon = new Button
            {
                Content = "👁",
                Tag = file,
                Margin = new Thickness(5),
                Width = 30
            };
            eyeIcon.Click += EyeIcon_Click;

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            stackPanel.Children.Add(fileItem);

            stackPanel.Children.Add(eyeIcon);

            FileListBox.Items.Add(stackPanel);
        }
    }

    private void EyeIcon_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;

        var filePath = button.Tag.ToString();

        string fileType = Path.GetExtension(filePath);

        IFileLoader loader = LoaderFactory.GetLoader(fileType);

        List<TradeData> data = loader?.Load(filePath);

        FileDetailsDataGrid.ItemsSource = data;
    }

    private void SelectFolder_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            CheckFileExists = false,
            CheckPathExists = true,
            FileName = "Select a file",
            Title = "Select a File to Choose the Folder",
            Filter = "All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string selectedFilePath = openFileDialog.FileName;
            string selectedFolderPath = Path.GetDirectoryName(selectedFilePath);
            InputDirectoryTextBox.Text = selectedFolderPath;
        }
    }

    private void UpdateSettings_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int pollingInterval = 5;
            bool isAnyChangesExist = false;

            if (PollingIntervalTextBox.Text != string.Empty)
            {
                pollingInterval = int.Parse(PollingIntervalTextBox.Text);

                isAnyChangesExist = true;
            }

            string newDirectory = "C:\\Users\\User\\Desktop\\FileReader\\Files";

            if (InputDirectoryTextBox.Text != string.Empty)
            {
                newDirectory = InputDirectoryTextBox.Text;

                isAnyChangesExist = true;
            }



            _watcher?.StopWatching();

            _watcher.StartWatching(newDirectory, pollingInterval);

            LoadFiles(newDirectory);

            if (isAnyChangesExist)
            {

                MessageBox.Show("Settings updated successfully.");

            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating settings: {ex.Message}");
        }
    }
}