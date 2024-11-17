namespace FilesWordCount.Extensions;

public static class StringExtensions
{
    public static string AlignLeft(this string incomingText, int maxLength, char addingSymbol = ' ')
    {
        if (incomingText.Length > maxLength)
        {
            return incomingText[..(maxLength - 1)] + '~';
        }

        if (incomingText.Length < maxLength)
        {
            return incomingText.PadRight(maxLength, addingSymbol);
        }

        return incomingText;
    }

    public static string AlignRight(this string incomingText, int maxLength, char addingSymbol = ' ')
    {
        if (incomingText.Length > maxLength)
        {
            return '~' + incomingText[(incomingText.Length - maxLength - 1)..];
        }

        if (incomingText.Length < maxLength)
        {
            return incomingText.PadLeft(maxLength, addingSymbol);
        }

        return incomingText;
    }
}