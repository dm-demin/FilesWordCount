using FilesWordCount.Controllers;
using FilesWordCount.Enums;

namespace FilesWordCount.Helpers;

/// <summary>
/// Helps select concrete controller method depends on application executing mode.
/// </summary>
public static class ApplicationControllerRouter
{
    /// <summary>
    /// Select controller method for once statistic calculation.
    /// </summary>
    /// <param name="mode">Application mode.</param>
    /// <param name="controller">Concrete controller.</param>
    /// <returns>Delegate.</returns>
    public static ProcessOnce GetProcessMethod(ApplicationMode mode, ApplicationController controller)
    {
        return controller.ProcessFolder;
    }

    /// <summary>
    /// Select controller method for folder monitoring and statistic recalculation.
    /// </summary>
    /// <param name="mode">Application mode.</param>
    /// <param name="controller">Concrete controller.</param>
    /// <returns>Delegate.</returns>
    public static Monitor GetMonitorMethod(ApplicationMode mode, ApplicationController controller)
    {
        return controller.MonitorFolder;
    }
}

/// <summary>
/// Delegate type for one statistic calculation in <param name="path"> directory.
/// </summary>
public delegate void ProcessOnce(string path);

/// <summary>
/// Delegate type for monitoring <param name="path"> directory and recalculate statistic.
/// </summary>
public delegate void Monitor(string path, CancellationToken cancellationToken);
