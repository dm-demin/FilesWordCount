using FilesWordCount.Interfaces;
using FilesWordCount.Models;

namespace FilesWordCount.Providers;

/// <inheritdoc/>
public class FileStatisticMonitor : IMonitor
{
    private readonly IResultPublisher _resultPublisher;

    private readonly IStatisticProvider _statisticProvider;

    private readonly StatisticCalculationResult _results;

    #region constructor

    public FileStatisticMonitor(IStatisticProvider statisticProvider, IResultPublisher resultPublisher)
    {
        _results = new StatisticCalculationResult();
        _resultPublisher = resultPublisher;
        _statisticProvider = statisticProvider;
    }

    #endregion

    #region public methods

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

    private void OnFileChanged(object sender, FileSystemEventArgs eventArgs)
    {
        _results.Plus(_statisticProvider.AnalyzeFile(eventArgs.FullPath));

        PrintTop10(_results);
    }

    private void OnFileCreated(object sender, FileSystemEventArgs eventArgs)
    {
        _results.Plus(_statisticProvider.AnalyzeFile(eventArgs.FullPath));

        PrintTop10(_results);
    }

    private void OnFileDeleted(object sender, FileSystemEventArgs eventArgs)
    {
        _results.Remove(eventArgs.FullPath);

        PrintTop10(_results);
    }

    private void OnFileRenamed(object sender, RenamedEventArgs eventArgs)
    {
        _results.Remove(eventArgs.OldFullPath);
        _results.Plus(_statisticProvider.AnalyzeFile(eventArgs.FullPath));

        PrintTop10(_results);
    }

    private void PrintTop10(StatisticCalculationResult data)
    {
        _resultPublisher.Show(
            data.GetTop());
    }

    #endregion
}
