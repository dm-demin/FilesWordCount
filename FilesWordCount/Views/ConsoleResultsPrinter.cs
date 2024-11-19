using System.Diagnostics;
using FilesWordCount.Extensions;
using FilesWordCount.Interfaces;

namespace FilesWordCount.Controllers;

public class ConsoleResultsPrinter : IResultPublisher
{
    private static readonly object _lock = new ();

    /// <inheritdoc/>
    public void Show(IEnumerable<(string, string)> values, string keyColumnName, string valueColumnName)
    {
        lock (_lock)
        {
            if (!Debugger.IsAttached)
            {
                Console.Clear();
            }

            Console.WriteLine($"| {keyColumnName.AlignLeft(40)} | {valueColumnName.AlignLeft(20)} |");
            Console.WriteLine("|------------------------------------------+----------------------|");

            foreach ((string key, string value) item in values)
            {
                Console.WriteLine($"| {item.key.AlignLeft(40)} | {item.value.ToString().AlignRight(20)} |");
            }

            Console.WriteLine("|------------------------------------------+----------------------|");

            Console.WriteLine($"\nLast update: {DateTime.Now}");
            Console.WriteLine("To exit press Enter...");
        }
    }
}
