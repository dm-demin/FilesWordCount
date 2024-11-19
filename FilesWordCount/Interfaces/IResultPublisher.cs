using FilesWordCount.Models;

namespace FilesWordCount.Interfaces;

/// <summary>
/// Provide representation of analysis result.
/// </summary>
public interface IResultPublisher
{
    /// <summary>
    /// Display files statistics.
    /// </summary>
    /// <param name="files">Collection of files metadata.</param>
    void Show(IEnumerable<FileMetadata> files);
}
