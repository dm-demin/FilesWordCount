using FilesWordCount.Extensions;
using FilesWordCount.Helpers;
using FilesWordCount.Interfaces;

namespace FilesWordCount.Providers;

/// <inheritdoc/>
public class WordStatisticProvider : IStatisticProvider
{
    private readonly IResultPublisher _resultPublisher;

    private Dictionary<string, int> _results;

    #region constructor

    public WordStatisticProvider(IResultPublisher resultPublisher)
    {
        _resultPublisher = resultPublisher;
        _results = new Dictionary<string, int>();
    }

    #endregion

    #region public methods

    /// <inheritdoc/>
    public void AnalyzeFolder(string path)
    {
        _results = new Dictionary<string, int>();

        foreach (string filename in Directory.EnumerateFiles(path, "*.txt"))
        {
            _results = _results.Plus(AnalyzeFile(filename));
        }

        PrintTop10(_results);
    }

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

    private static Dictionary<string, int> AnalyzeFile(string filePath)
    {
        string? content = null;

        for (int i = 0; i < 10; i++)
        {
            try
            {
                content = File.ReadAllText(filePath);
                break;
            }
            catch (IOException)
            {
                Thread.Sleep(1000);
            }
        }

        if (content == null)
        {
            throw new IOException("Cannot read file content");
        }

        return WordStatistic.Get(content);
    }

    private void OnFileChanged(object sender, FileSystemEventArgs eventArgs)
    {
        AnalyzeFolder(eventArgs.FullPath.Replace(eventArgs.Name ?? string.Empty, string.Empty));

        PrintTop10(_results);
    }

    private void OnFileCreated(object sender, RenamedEventArgs eventArgs)
    {
        _results.Plus(AnalyzeFile(eventArgs.FullPath));
    }

    private void PrintTop10(IDictionary<string, int> data)
    {
        _resultPublisher.Show(
            data.OrderByDescending(x => x.Value).Select(x => (x.Key, x.Value.ToString())).Take(10),
            "Word",
            "Total count");
    }

    #endregion
}
