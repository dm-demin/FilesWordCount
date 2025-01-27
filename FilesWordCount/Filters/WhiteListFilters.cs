namespace FilesWordCount.Filters;

/// <summary>
/// Filtering incoming word using white list.
/// </summary>
public static class WhiteListFilters
{
    private static readonly List<string> _whiteList = ["Наташа", "Пьер", "Андрей"];

    /// <summary>
    /// Present in white list.
    /// </summary>
    /// <param name="inputWord">Analyzing word.</param>
    /// <returns>true when input word is presented in white list.</returns>
    public static bool ExistInWhiteList(string inputWord)
    {
        return _whiteList.Contains(inputWord);
    }
}
