using FilesWordCount.Models;

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
    StatisticCalculationResult AnalyzeFolder(string path);

    /// <summary>
    /// This method calculate statistics for file.
    /// </summary>
    /// <param name="path">Directory path to process files.</param>
    StatisticCalculationResult AnalyzeFile(string path);
}
