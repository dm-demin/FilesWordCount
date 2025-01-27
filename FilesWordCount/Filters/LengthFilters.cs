namespace FilesWordCount.Filters;

/// <summary>
/// Text length filters/
/// </summary>
public class LengthFilter
{
    /// <summary>
    /// Minimal length filter
    /// </summary>
    /// <param name="inputText">Analyze string.</param>
    /// <returns>true when input text length more or equals 5 symbols</returns>
    public static bool MinimumFiveSymbol(string inputText)
    {
        return inputText.Length >= 5;
    }
}
