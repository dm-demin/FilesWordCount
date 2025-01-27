using FilesWordCount.Interfaces;

namespace FilesWordCount.Utils;

/// <summary>
/// Application context class.
/// Store providers realization.
/// </summary>
public class ApplicationContext
{
    /// <summary>
    /// Statistic provider class.
    /// </summary>
    public IStatisticProvider StatisticProvider { get; set; }

    /// <summary>
    /// Result publisher class.
    /// </summary>
    public IResultPublisher ResultPublisher { get; set; }

    /// <summary>
    /// Monitor class for watching files changes.
    /// </summary>
    public IMonitor FolderMonitor { get; set; }
}
