using System.Diagnostics;
using FilesWordCount.Extensions;
using FilesWordCount.Interfaces;

namespace FilesWordCount.Services.Publishers;

public class ConsoleResultPublisher : IResultPublisher
{
    private static readonly object _lock = new ();
    private readonly string _keyColumnName;
    private readonly string _valueColumnName;

    public ConsoleResultPublisher()
    {
        _keyColumnName = "Word";
        _valueColumnName = "Count";
    }

    public ConsoleResultPublisher(string keyColumnName, string valueColumnName)
    {
        _keyColumnName = keyColumnName;
        _valueColumnName = valueColumnName;
    }

    /// <inheritdoc/>
    public void Show(IEnumerable<(string, int)> values)
    {
        lock (_lock)
        {
            if (!Debugger.IsAttached)
            {
                Console.Clear();
            }

            Console.WriteLine($"| {_keyColumnName.AlignLeft(40)} | {_valueColumnName.AlignLeft(20)} |");
            Console.WriteLine("|------------------------------------------+----------------------|");

            foreach ((string key, int value) item in values)
            {
                Console.WriteLine($"| {item.key.AlignLeft(40)} | {item.value.ToString().AlignRight(20)} |");
            }

            Console.WriteLine("|------------------------------------------+----------------------|");

            Console.WriteLine($"\nLast update: {DateTime.Now}");
            Console.WriteLine("To exit press Enter...");
        }
    }
}
