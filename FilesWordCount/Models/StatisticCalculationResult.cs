using FilesWordCount.Extensions;

namespace FilesWordCount.Models;

/// <summary>
/// Model for statistic calculation provider result.
/// </summary>
public class StatisticCalculationResult
{
    private Dictionary<string, int> _statistic;

    /// <summary>
    /// Constructor.
    /// </summary>
    public StatisticCalculationResult()
    {
        _statistic = new Dictionary<string, int>();
    }

    /// <summary>
    /// Concat this statistic collection with <param name="other"> collection.
    /// </summary>
    public StatisticCalculationResult Plus(StatisticCalculationResult other)
    {
        _statistic = _statistic.Plus(other._statistic);
        return this;
    }

    /// <summary>
    /// Remove <param name="entityKey"> from this statistic collection.
    /// </summary>
    public StatisticCalculationResult Remove(string entityKey)
    {
        _statistic.Remove(entityKey);
        return this;
    }

    /// <summary>
    /// Add parameter <param name="key"> with its value <param name="value"> to result statistics
    /// </summary>
    public StatisticCalculationResult Add(string key, int value)
    {
        _statistic.Add(key, value);
        return this;
    }

    /// <summary>
    /// Add <param name="entity"> statistic record to result.
    /// </summary>
    public StatisticCalculationResult Add(KeyValuePair<string, int> entity)
    {
        _statistic.Add(key: entity.Key, value: entity.Value);
        return this;
    }

    /// <summary>
    /// Return TOP (sorted by values descending, by default 10) items. Count could be replaced with <param name="count"> parameter.
    /// </summary>
    public IEnumerable<(string, int)> GetTop(int count = 3)
    {
        return _statistic.OrderByDescending(x => x.Value).Select(x => (x.Key, x.Value)).Take(count);
    }
}
