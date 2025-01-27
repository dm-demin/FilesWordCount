using System.Diagnostics;
using FilesWordCount.Controllers;
using FilesWordCount.Enums;
using FilesWordCount.Helpers;
using FilesWordCount.Interfaces;
using FilesWordCount.Services.Publishers;
using FilesWordCount.Utils;

IResultPublisher resultPublisher = new ConsoleResultPublisher();

string paramMode;
string path = Directory.GetCurrentDirectory();
ApplicationMode mode = ApplicationMode.JsonWithBlackList;

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
    paramMode = args[0];

    mode = paramMode switch
    {
        "1" => ApplicationMode.Console,
        "2" => ApplicationMode.JsonWithWhiteList,
        "3" => ApplicationMode.JsonWithBlackList,
        _ => throw new Exception("Incorrect command")
    };
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

ApplicationContext applicationContext = ApplicationContextHelper.GetApplicationContext(mode);

ApplicationController controller = new ApplicationController(
    applicationContext.StatisticProvider,
    applicationContext.ResultPublisher,
    applicationContext.FolderMonitor);

controller.ProcessFolder(path);

CancellationTokenSource cts = new ();

#pragma warning disable CS4014 // background task starting.
Task.Run(() => controller.MonitorFolder(path, cts.Token));

// To exit press Enter...
Console.ReadLine();

cts.Cancel();

Console.WriteLine("Trying cancel background task...");
Thread.Sleep(2000);

cts.Dispose();
