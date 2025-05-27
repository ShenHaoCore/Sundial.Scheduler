using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;

namespace Sundial.Scheduler.Quartz;

/// <summary>
/// 
/// </summary>
public class QuartzOption
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="config"></param>
    public QuartzOption(IConfiguration config)
    {
        var section = config.GetSection("Quartz");
        section.Bind(this);
    }

    /// <summary>
    /// 
    /// </summary>
    public Scheduler Scheduler { get; set; } = new Scheduler();

    /// <summary>
    /// 
    /// </summary>
    public ThreadPool ThreadPool { get; set; } = new ThreadPool();

    /// <summary>
    /// 
    /// </summary>
    public Plugin Plugin { get; set; } = new Plugin();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public NameValueCollection ToProperties()
    {
        var properties = new NameValueCollection
        {
            ["quartz.scheduler.instanceName"] = Scheduler?.InstanceName,
            ["quartz.threadPool.type"] = ThreadPool?.Type,
            ["quartz.threadPool.threadPriority"] = ThreadPool?.ThreadPriority,
            ["quartz.threadPool.threadCount"] = ThreadPool?.ThreadCount.ToString(),
            ["quartz.plugin.jobInitializer.type"] = Plugin?.JobInitializer?.Type,
            ["quartz.plugin.jobInitializer.fileNames"] = Plugin?.JobInitializer?.FileNames
        };
        return properties;
    }
}

/// <summary>
/// 
/// </summary>
public class Scheduler
{
    /// <summary>
    /// 
    /// </summary>
    public string InstanceName { get; set; } = string.Empty;
}

/// <summary>
/// 
/// </summary>
public class ThreadPool
{
    /// <summary>
    /// 
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string ThreadPriority { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public int ThreadCount { get; set; } = 10;
}

/// <summary>
/// 
/// </summary>
public class Plugin
{
    public JobInitializer JobInitializer { get; set; } = new JobInitializer();
}

/// <summary>
/// 
/// </summary>
public class JobInitializer
{
    /// <summary>
    /// 
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string FileNames { get; set; } = string.Empty;
}
