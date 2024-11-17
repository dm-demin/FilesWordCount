namespace FilesWordCount.Interfaces;

/// <summary>
/// Calculate statistics for files in specific folder/
/// </summary>
public interface IStatisticProvider
{
    /// <summary>
    /// This method calculate statistics for each file in directory just one time.
    /// </summary>
    /// <param name="path">Directory path to process files.</param>
    void AnalyzeFolder(string path);

    /// <summary>
    /// This method subscribe to <paramref name="path"> directory to process files</paramref> Each changes starts updating statistics.
    /// </summary>
    /// <param name="path">Directory path to process files.</param>
    void MonitorFolder(string path, CancellationToken cancellationToken);
}
