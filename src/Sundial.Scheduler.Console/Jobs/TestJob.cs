using Microsoft.Extensions.Logging;
using Quartz;

namespace Sundial.Scheduler.Jobs;

/// <summary>
/// 测试作业
/// </summary>
public class TestJob : IJob
{
    private readonly ILogger<TestJob> _logger;

    /// <summary>
    /// 测试作业
    /// </summary>
    /// <param name="logger"></param>
    public TestJob(ILogger<TestJob> logger)
    {
        this._logger = logger;
    }

    /// <summary>
    /// 执行作业
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"任务[{nameof(TestJob)}开始执行时间[{DateTime.Now}]");
        return Task.CompletedTask;
    }
}
