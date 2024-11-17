using FilesWordCount.Models;

namespace FilesWordCount.Interfaces;

public interface IResultPublisher
{
    /// <summary>
    /// Display files statistics.
    /// </summary>
    /// <param name="files">Collection of files metadata.</param>
    void Show(IEnumerable<FileMetadata> files);
}
