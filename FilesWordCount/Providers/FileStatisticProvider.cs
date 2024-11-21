using FilesWordCount.Helpers;
using FilesWordCount.Interfaces;
using FilesWordCount.Models;

namespace FilesWordCount.Providers;

/// <inheritdoc/>
public class FileStatisticProvider : IStatisticProvider
{
    private readonly IResultPublisher _resultPublisher;

    private Dictionary<string, FileMetadata> _results;

    #region constructor

    public FileStatisticProvider(IResultPublisher resultPublisher)
    {
        _resultPublisher = resultPublisher;
        _results = new Dictionary<string, FileMetadata>();
    }

    #endregion

    #region public methods

    /// <inheritdoc/>
    public void AnalyzeFolder(string path)
    {
        var filenames = Directory.EnumerateFiles(path, "*.txt");

        _results = filenames.Select(x => AnalyzeFile(x)).ToDictionary(x => x.Path);

        PrintTop10(_results.Values);
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

    private static FileMetadata AnalyzeFile(string filePath)
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

        return new FileMetadata
        {
            Path = filePath,
            FileName = Path.GetFileName(filePath),
            WordCount = WordCounter.Count(content)
        };
    }

    private void OnFileChanged(object sender, FileSystemEventArgs eventArgs)
    {
        FileMetadata fileMetadata = AnalyzeFile(eventArgs.FullPath);

        _results[fileMetadata.Path] = fileMetadata;

        PrintTop10(_results.Values);
    }

    private void OnFileCreated(object sender, FileSystemEventArgs eventArgs)
    {
        FileMetadata fileMetadata = AnalyzeFile(eventArgs.FullPath);

        _results.Add(fileMetadata.Path, fileMetadata);

        PrintTop10(_results.Values);
    }

    private void OnFileDeleted(object sender, FileSystemEventArgs eventArgs)
    {
        _results.Remove(eventArgs.FullPath);

        PrintTop10(_results.Values);
    }

    private void OnFileRenamed(object sender, RenamedEventArgs eventArgs)
    {
        FileMetadata fileMetadata = AnalyzeFile(eventArgs.FullPath);

        _results.Add(eventArgs.FullPath, fileMetadata);
        _results.Remove(eventArgs.OldFullPath);

        PrintTop10(_results.Values);
    }

    private void PrintTop10(IEnumerable<FileMetadata> data)
    {
        _resultPublisher.Show(
            data.OrderByDescending(x => x.WordCount).Select(x => (x.FileName, x.WordCount.ToString())).Take(10),
            "Filename",
            "Words count");
    }

    #endregion
}
