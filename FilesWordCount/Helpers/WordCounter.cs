namespace FilesWordCount.Helpers;

public static class WordCounter
{
    public static int Count(string text, byte minWordLength = 3, string delimiter = " ")
    {
        if (text.Length < minWordLength)
        {
            return 0;
        }

        return text.Replace("\n", delimiter)
                    .Split(delimiter)
                    .Where(x => x.Length >= minWordLength).Count();
    }
}
