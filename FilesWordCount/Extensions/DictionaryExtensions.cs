namespace FilesWordCount.Extensions;

public static class DictionaryExtensions
{
    public static Dictionary<string, int> Plus(this Dictionary<string, int> original, Dictionary<string, int> additional)
    {
        return original.Concat(additional)
                                .GroupBy(x => x.Key)
                                .Select(x => new { Word = x.Key, Count = x.Sum(opt => opt.Value) })
                                .ToDictionary(x => x.Word, x => x.Count);
    }
}
