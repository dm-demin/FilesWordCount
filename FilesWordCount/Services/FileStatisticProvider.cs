using FilesWordCount.Interfaces;
using FilesWordCount.Models;

namespace FilesWordCount.Providers;

/// <inheritdoc/>
public class FileStatisticProvider : IStatisticProvider
{
    private readonly Func<string, bool> _filter;
    private readonly string _delimiter;

    #region constructor

    public FileStatisticProvider(Func<string, bool> filter, string delimiter = "")
    {
        _filter = filter;
        _delimiter = delimiter;
    }

    #endregion

    #region public methods

    /// <inheritdoc/>
    public StatisticCalculationResult AnalyzeFolder(string path)
    {
        StatisticCalculationResult results = new ();

        foreach (string filename in Directory.EnumerateFiles(path, "*.txt"))
        {
            results = results.Plus(AnalyzeFile(filename));
        }

        return results;
    }

    /// <inheritdoc/>
    public StatisticCalculationResult AnalyzeFile(string filePath)
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

        StatisticCalculationResult result = new ();
        result.Add(
            key: Path.GetFileName(filePath),
            value: content.Replace("\n", _delimiter)
                .Split(_delimiter)
                .Where(_filter).Count());

        return result;
    }

    #endregion
}
