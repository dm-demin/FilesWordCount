namespace FilesWordCount.Interfaces;

/// <summary>
/// Provide representation of analysis result.
/// </summary>
public interface IResultPublisher
{
    /// <summary>
    /// Display files statistics.
    /// </summary>
    /// <param name="values">Collection of files metadata.</param>
    void Show(IEnumerable<(string, string)> values, string keyColumnName, string valueColumnName);
}
