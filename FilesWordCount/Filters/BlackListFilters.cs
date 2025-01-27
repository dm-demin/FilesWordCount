namespace FilesWordCount.Filters;

/// <summary>
/// Filtering incoming words using black list.
/// </summary>
public static class BlackListFilters
{
    private static readonly List<string> _blackList = ["Наташа", "Пьер", "Андрей"];

    /// <summary>
    /// Black list filter.
    /// </summary>
    /// <param name="inputWord">Analyzing word.</param>
    /// <returns>true when input word not exist in black list</returns>
    public static bool NotInBlackList(string inputWord)
    {
        return !_blackList.Contains(inputWord) && inputWord.Length > 4;
    }
}
