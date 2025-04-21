namespace Sundial.Scheduler.Models;

/// <summary>
/// 
/// </summary>
public class QuartzConfig
{
    /// <summary>
    /// 
    /// </summary>
    public List<QuartzJobConfig> Jobs { get; set; } = [];
}

/// <summary>
/// QUARTZ作业
/// </summary>
public class QuartzJobConfig
{
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Group { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string JobType { get; set; } = string.Empty;
}
