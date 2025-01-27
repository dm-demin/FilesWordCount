namespace FilesWordCount.Models.Response;

public class StatisticRecord
{
    public string Word { get; set; }
    public int Count { get; set; }
}

public class WordStatisticResponse
{
    public List<StatisticRecord> Statistics { get; set; }
}
