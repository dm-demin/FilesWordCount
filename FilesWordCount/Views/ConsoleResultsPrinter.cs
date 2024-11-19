using System.Diagnostics;
using FilesWordCount.Extensions;
using FilesWordCount.Interfaces;
using FilesWordCount.Models;

namespace FilesWordCount.Controllers;

public class ConsoleResultsPrinter : IResultPublisher
{
    private static readonly object _lock = new ();

    /// <inheritdoc/>
    public void Show(IEnumerable<FileMetadata> data)
    {
        lock (_lock)
        {
            if (!Debugger.IsAttached)
            {
                Console.Clear();
            }

            Console.WriteLine("| Filename                                 | Words count          |");
            Console.WriteLine("|------------------------------------------+----------------------|");

            foreach (FileMetadata item in data)
            {
                Console.WriteLine($"| {item.FileName.AlignLeft(40)} | {item.WordCount.ToString().AlignRight(20)} |");
            }

            Console.WriteLine("|------------------------------------------+----------------------|");

            Console.WriteLine($"\nLast update: {DateTime.Now}");
            Console.WriteLine("To exit press Enter...");
        }
    }
}
