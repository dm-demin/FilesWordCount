using System.Diagnostics;
using FilesWordCount.Controllers;
using FilesWordCount.Interfaces;
using FilesWordCount.Providers;

IResultPublisher resultPublisher = new ConsoleResultsPrinter();
IStatisticProvider statisticProvider;

string mode = "word";
string path = Directory.GetCurrentDirectory();

if (args.Length < 1 && !Debugger.IsAttached)
{
    Console.WriteLine("mode [folderpath]");
    Console.WriteLine("mode: file - calculate total count of word in each file and print top10 files by word count");
    Console.WriteLine("         word - print top10 word in all files in directory ");
    Console.WriteLine("folderpath - full path o analyzed folder, if not set current directory will be used");
    return;
}

if (args.Length >= 1 && !Debugger.IsAttached)
{
    mode = args[0];
}

if (args.Length >= 2 && !Debugger.IsAttached)
{
    path = args[1];
}

if (!Directory.Exists(path))
{
    Console.WriteLine("Directory not exist...");
    return;
}

if (Debugger.IsAttached)
{
    path = "..\\..\\..\\..\\files";
}

statisticProvider = mode switch
{
    "file" => new FileStatisticProvider(resultPublisher),
    "word" => new WordStatisticProvider(resultPublisher),
    _ => throw new Exception("Incorrect command")
};

statisticProvider.AnalyzeFolder(path);

CancellationTokenSource cts = new ();

#pragma warning disable CS4014 // background task starting.
Task.Run(() => statisticProvider.MonitorFolder(path, cts.Token));

// To exit press Enter...
Console.ReadLine();

cts.Cancel();

Console.WriteLine("Trying cancel background task...");
Thread.Sleep(2000);

cts.Dispose();
