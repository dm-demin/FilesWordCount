using FilesWordCount.Interfaces;
using FilesWordCount.Models;

namespace FilesWordCount.Providers;

/// <inheritdoc/>
public class WordStatisticProvider : IStatisticProvider
{
    private static readonly List<string> _delimiters = ["!", ".", ",", "?", "'", "-", "\"", "\r\n", "\n", "\t", " "];
    private readonly Func<string, bool> _filter;
    private readonly string _delimiter;

    #region constructor

    public WordStatisticProvider(Func<string, bool> filter, string delimiter = " ")
    {
        _delimiter = delimiter;
        _filter = filter;
    }

    #endregion

    #region public methods

    /// <inheritdoc/>
    public StatisticCalculationResult AnalyzeFolder(string path)
    {
        StatisticCalculationResult results = new ();

        foreach (string filename in Directory.EnumerateFiles(path, "*.txt"))
        {
            results.Plus(AnalyzeFile(filename));
        }

        return results;
    }

    /// <inheritdoc/>
    public StatisticCalculationResult AnalyzeFile(string filePath)
    {
        string? content = null;
        StatisticCalculationResult result = new ();

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

        _delimiters.ForEach(symbol => content = content.Replace(symbol, _delimiter));

        content.Split(_delimiter)
                    .Where(_filter)
                    .Select(x => x.ToLower())
                    .GroupBy(x => x)
                    .ToList()
                    .ForEach(x => result = result.Add(x.Key, x.Count()));

        return result;
    }

    #endregion

}
