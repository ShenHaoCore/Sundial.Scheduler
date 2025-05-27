using Microsoft.Extensions.Logging;
using Quartz;

namespace Sundial.Scheduler.Jobs;

/// <summary>
/// 
/// </summary>
public class BilibiliJob : IJob
{
    private readonly ILogger<BilibiliJob> _logger;

    /// <summary>
    /// 测试作业
    /// </summary>
    /// <param name="logger"></param>
    public BilibiliJob(ILogger<BilibiliJob> logger)
    {
        this._logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"任务[{nameof(BilibiliJob)}]开始执行时间[{DateTime.Now}]");
        return Task.CompletedTask;
    }
}
