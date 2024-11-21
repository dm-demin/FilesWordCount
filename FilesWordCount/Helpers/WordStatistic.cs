namespace FilesWordCount.Helpers;

public static class WordStatistic
{
    private static readonly List<string> _delimiters = ["!", ".", ",", "?", "'", "-", "\"", "\r\n", "\n", "\t", " "];
    public static Dictionary<string, int> Get(string text, byte minWordLength = 3, string delimiter = " ")
    {
        if (text.Length < minWordLength)
        {
            return new Dictionary<string, int>();
        }

        _delimiters.ForEach(symbol => text = text.Replace(symbol, delimiter));

        return text.Split(delimiter)
                    .Where(x => x.Length >= minWordLength)
                    .Select(x => x.ToLower())
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count());
    }
}
