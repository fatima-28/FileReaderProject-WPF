using System.IO;
using System.Windows.Threading;

namespace FileReaderProject.Services;   

public class DirectoryWatcher
{
    private DispatcherTimer _timer;

    private string _directoryPath;

    private HashSet<string> _knownFiles;

    public string DirectoryPath => _directoryPath;

    public event Action<string> FileChanged;

    public void StartWatching(string directoryPath, int pollingIntervalSeconds)
    {
        _directoryPath = directoryPath;

        _knownFiles = new HashSet<string>(Directory.GetFiles(directoryPath));

        if (_timer != null)
        {
            _timer.Stop();
            _timer.Tick -= OnTimerTick;
            _timer = null;
        }

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(pollingIntervalSeconds)
        };
        _timer.Tick += OnTimerTick;

        _timer.Start();
    }

    private void OnTimerTick(object sender, EventArgs e)
    {
        var currentFiles = new HashSet<string>(Directory.GetFiles(_directoryPath));

        var addedFiles = currentFiles.Except(_knownFiles).ToList();

        var removedFiles = _knownFiles.Except(currentFiles).ToList();

        if (addedFiles.Any() || removedFiles.Any())
        {
            foreach (var file in addedFiles)
            {
                FileChanged?.Invoke(file);
            }

            foreach (var file in removedFiles)
            {
                FileChanged?.Invoke(file);
            }

            _knownFiles = currentFiles;
        }
    }

    public void StopWatching()
    {
        _timer?.Stop();
    }
}
