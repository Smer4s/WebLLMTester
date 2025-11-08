namespace Uply.Domain.Enums;

public enum Period
{
    /// <summary>
    ///     Каждый день
    /// </summary>
    Daily,

    /// <summary>
    ///     Один раз в 2 дня
    /// </summary>
    Every2Days,

    /// <summary>
    ///     Один раз в 3 дня
    /// </summary>
    Every3Days,

    /// <summary>
    ///     Один раз в 4 дня
    /// </summary>
    Every4Days,

    /// <summary>
    ///     Один раз в 5 дней
    /// </summary>
    Every5Days
}

public static class PeriodExtensions
{
    public static int ToDaysCount(this Period period) 
        => period switch
        {
            Period.Daily => 1,
            Period.Every2Days => 2,
            Period.Every3Days => 3,
            Period.Every4Days => 4,
            Period.Every5Days => 5,
            _ => throw new ArgumentException(null, nameof(period))
        };
}