namespace FilesWordCount.Models;

public struct FileMetadata
{
    public string FileName { get; set; }
    public string Path { get; set; }
    public int WordCount { get; set; }
}