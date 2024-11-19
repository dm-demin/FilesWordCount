using System.Diagnostics;
using FilesWordCount.Controllers;
using FilesWordCount.Interfaces;
using FilesWordCount.Providers;

IResultPublisher resultPublisher = new ConsoleResultsPrinter();
IStatisticProvider statisticProvider = new StatisticProvider(resultPublisher);

string path = Directory.GetCurrentDirectory();

if (args.Length > 0)
{
    path = args[0];
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
