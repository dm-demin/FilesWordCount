using System.Diagnostics;
using FilesWordCount.Extensions;
using FilesWordCount.Interfaces;
using FilesWordCount.Models;

namespace FilesWordCount.Controllers;

public class ConsoleResultsController : IResultPublisher
{
    /// <inheritdoc/>
    public void Show(IEnumerable<FileMetadata> results)
    {
        if(!Debugger.IsAttached)
        {
            Console.Clear();
        }        

        Console.WriteLine("| Filename                                 | Words count          |");
        Console.WriteLine("|------------------------------------------+----------------------|");

        var items = results.OrderByDescending(item => item.WordCount).ToList();
        items.ForEach(item => 
            Console.WriteLine($"| {item.FileName.AlignLeft(40)} | {item.WordCount.ToString().AlignRight(20)} |")
        );
        Console.WriteLine("|------------------------------------------+----------------------|");

        Console.WriteLine($"\nLast update: {DateTime.Now}");
        Console.WriteLine("To exit press Enter...");

    }
}
