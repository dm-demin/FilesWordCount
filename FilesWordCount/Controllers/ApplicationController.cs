using FilesWordCount.Interfaces;
using FilesWordCount.Models;

namespace FilesWordCount.Controllers;

public class ApplicationController
{
    private readonly IStatisticProvider _statisticProvider;
    private readonly IResultPublisher _resultPublisher;
    private readonly IMonitor _monitor;

    public ApplicationController(IStatisticProvider statisticProvider, IResultPublisher resultPublisher, IMonitor monitor)
    {
        _statisticProvider = statisticProvider;
        _resultPublisher = resultPublisher;
        _monitor = monitor;
    }

    public void ProcessFolder(string path)
    {
        StatisticCalculationResult statistic = _statisticProvider.AnalyzeFolder(path);

        _resultPublisher.Show(statistic.GetTop());
    }

    public void MonitorFolder(string path, CancellationToken cancellationToken)
    {
        _monitor.MonitorFolder(path, cancellationToken);
    }
}
