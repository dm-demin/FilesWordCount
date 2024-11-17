using FilesWordCount.Helpers;
using FilesWordCount.Interfaces;
using FilesWordCount.Models;

namespace FilesWordCount.Providers;

/// <inheritdoc/>
public class StatisticProvider : IStatisticProvider
{
    private readonly IResultPublisher _resultPublisher;

    private readonly Dictionary<string, FileMetadata> _results;

    #region constructor

    public StatisticProvider(IResultPublisher resultPublisher)
    {
        _resultPublisher = resultPublisher;
        _results = new Dictionary<string, FileMetadata>();
    }

    #endregion

    #region public methods

    /// <inheritdoc/>
    public void AnalyzeFolder(string path)
    {
        List<string> filenames = Directory.EnumerateFiles(path).ToList();

        filenames.ForEach(async filename =>
        {
            FileMetadata fileMetadata = await AnalyzeFileAsync(filename);

            lock (_results)
            {
                _results.Add(fileMetadata.Path, fileMetadata);
            }
        });

        ShowStatistic();
    }

    /// <inheritdoc/>
    public void MonitorFolder(string path, CancellationToken cancellationToken)
    {
        using FileSystemWatcher watcher = new FileSystemWatcher(path);

        watcher.Changed += OnFileChanged;
        watcher.Created += OnFileCreated;
        watcher.Deleted += OnFileDeleted;
        watcher.Renamed += OnFileRenamed;

        watcher.IncludeSubdirectories = false;
        watcher.EnableRaisingEvents = true;

        cancellationToken.WaitHandle.WaitOne();
    }

    #endregion

    #region private methods

    private static async Task<FileMetadata> AnalyzeFileAsync(string filePath)
    {
        string content = await File.ReadAllTextAsync(filePath);

        return new FileMetadata
        {
            Path = filePath,
            FileName = Path.GetFileName(filePath),
            WordCount = WordCounter.Count(content)
        };
    }

    private async void OnFileChanged(object sender, FileSystemEventArgs eventArgs)
    {
        FileMetadata fileMetadata = await AnalyzeFileAsync(eventArgs.FullPath);

        lock (_results)
        {
            _results[fileMetadata.Path] = fileMetadata;
        }

        ShowStatistic();
    }

    private async void OnFileCreated(object sender, FileSystemEventArgs eventArgs)
    {
        FileMetadata fileMetadata = await AnalyzeFileAsync(eventArgs.FullPath);

        lock (_results)
        {
            _results.Add(fileMetadata.Path, fileMetadata);
        }

        ShowStatistic();
    }

    private void OnFileDeleted(object sender, FileSystemEventArgs eventArgs)
    {
        lock (_results)
        {
            _results.Remove(eventArgs.FullPath);
        }

        ShowStatistic();
    }

    private async void OnFileRenamed(object sender, RenamedEventArgs eventArgs)
    {
        FileMetadata fileMetadata = await AnalyzeFileAsync(eventArgs.FullPath);

        lock (_results)
        {
            _results.Add(eventArgs.FullPath, fileMetadata);
            _results.Remove(eventArgs.OldFullPath);
        }

        ShowStatistic();
    }

    private void ShowStatistic()
    {
        _resultPublisher.Show(_results.Values);
    }

    #endregion
}
