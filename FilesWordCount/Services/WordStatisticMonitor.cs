using FilesWordCount.Interfaces;
using FilesWordCount.Models;

namespace FilesWordCount.Providers;

/// <inheritdoc/>
public class WordStatisticMonitor : IMonitor
{
    private readonly IResultPublisher _resultPublisher;

    private readonly IStatisticProvider _statisticProvider;

    private readonly StatisticCalculationResult _results;

    #region constructor

    public WordStatisticMonitor(IResultPublisher resultPublisher, IStatisticProvider statisticProvider)
    {
        _resultPublisher = resultPublisher;
        _results = new StatisticCalculationResult();
        _statisticProvider = statisticProvider;
    }

    #endregion

    #region public methods

    /// <inheritdoc/>
    public void MonitorFolder(string path, CancellationToken cancellationToken)
    {
        using FileSystemWatcher watcher = new FileSystemWatcher(path);

        watcher.Changed += OnFileChanged;
        watcher.Created += OnFileChanged;
        watcher.Deleted += OnFileChanged;

        watcher.IncludeSubdirectories = false;
        watcher.EnableRaisingEvents = true;

        cancellationToken.WaitHandle.WaitOne();
    }

    #endregion

    #region private methods

    private void OnFileChanged(object sender, FileSystemEventArgs eventArgs)
    {
        _statisticProvider.AnalyzeFolder(eventArgs.FullPath.Replace(eventArgs.Name ?? string.Empty, string.Empty));

        PrintTop10(_results);
    }

    private void OnFileCreated(object sender, RenamedEventArgs eventArgs)
    {
        _results.Plus(_statisticProvider.AnalyzeFile(eventArgs.FullPath));
    }

    private void PrintTop10(StatisticCalculationResult data)
    {
        _resultPublisher.Show(
            data.GetTop());
    }

    #endregion
}
