using AutoMapper;
using FilesWordCount.Enums;
using FilesWordCount.Filters;
using FilesWordCount.Interfaces;
using FilesWordCount.Mappers;
using FilesWordCount.Providers;
using FilesWordCount.Services.Publishers;
using FilesWordCount.Utils;

namespace FilesWordCount.Helpers;

/// <summary>
/// Helper for filling application context depends on executing mode.
/// </summary>
public static class ApplicationContextHelper
{
    /// <summary>
    /// Fill application context.
    /// </summary>
    /// <param name="mode">Executing mode.</param>
    /// <returns>ApplicationContext</returns>
    public static ApplicationContext GetApplicationContext(ApplicationMode mode)
    {
        IResultPublisher resultPublisher = GetResultPublisher(mode);

        IStatisticProvider statisticProvider = GetStatisticProvider(mode);

        return new ApplicationContext()
        {
            StatisticProvider = statisticProvider,
            ResultPublisher = resultPublisher,
            FolderMonitor = GetFolderMonitor(mode, statisticProvider, resultPublisher)
        };
    }

    private static IResultPublisher GetResultPublisher(ApplicationMode mode)
    {
        IMapper mapper = WordStatisticMapper.GetMapper();

        return mode switch {
            ApplicationMode.Console => new ConsoleResultPublisher(),
            ApplicationMode.JsonWithWhiteList => new JsonResultsPublisher(mapper),
            ApplicationMode.JsonWithBlackList => new JsonResultsPublisher(mapper),
            _ => throw new Exception("Cannot find result publisher for this executing mode")
        };
    }

    private static IStatisticProvider GetStatisticProvider(ApplicationMode mode)
    {
        Func<string, bool> filter = GetFilter(mode);

        return new WordStatisticProvider(filter);
    }

    private static IMonitor GetFolderMonitor(ApplicationMode mode, IStatisticProvider statisticProvider, IResultPublisher publisher)
    {
        List<ApplicationMode> wordProcessingModes = [ApplicationMode.JsonWithBlackList, ApplicationMode.JsonWithWhiteList, ApplicationMode.Console];

        if (!wordProcessingModes.Contains(mode))
        {
            throw new Exception("Cannot find folder monitor for given executing mode");
        }

        return new WordStatisticMonitor(publisher, statisticProvider);
    }

    private static Func<string, bool> GetFilter(ApplicationMode mode) => mode switch
    {
        ApplicationMode.Console => LengthFilter.MinimumFiveSymbol,
        ApplicationMode.JsonWithWhiteList => WhiteListFilters.ExistInWhiteList,
        ApplicationMode.JsonWithBlackList => BlackListFilters.NotInBlackList,
        _ => throw new Exception("Cannot find filtering predicate for this executing mode")
    };
}
