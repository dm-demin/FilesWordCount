namespace FilesWordCount.Interfaces;

/// <summary>
/// Monitoring directory changes to refresh statistics.
/// </summary>
public interface IMonitor
{
    /// <summary>
    /// This method subscribe to <paramref name="path"> directory to process files</paramref> Each changes starts updating statistics.
    /// </summary>
    /// <param name="path">Directory path to process files.</param>
    void MonitorFolder(string path, CancellationToken cancellationToken);
}
