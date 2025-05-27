using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Sundial.Scheduler.Models;
using Topshelf.Options;

namespace Sundial.Scheduler.Quartz;

/// <summary>
/// 
/// </summary>
public class QuartzService
{
    private readonly ILogger<QuartzService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly IScheduler _scheduler;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="configuration"></param>
    public QuartzService(ILogger<QuartzService> logger, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _scheduler = GetScheduler();
    }

    /// <summary>
    /// 启动
    /// </summary>
    /// <returns></returns>
    public async Task StartAsync()
    {
        _logger.LogInformation("服务正在启动...");
        await _scheduler.Start();
    }

    /// <summary>
    /// 停止
    /// </summary>
    public async Task StopAsync()
    {
        _logger.LogInformation("服务正在关闭...");
        await _scheduler.Shutdown();
    }

    /// <summary>
    /// 获取调度
    /// </summary>
    /// <returns></returns>
    private IScheduler GetScheduler()
    {
        QuartzOption option = new QuartzOption(_configuration);
        IScheduler scheduler = new StdSchedulerFactory(option.ToProperties()).GetScheduler().Result;
        scheduler.JobFactory = _serviceProvider.GetRequiredService<IJobFactory>();
        return scheduler;
    }

    /// <summary>
    /// 注册作业
    /// </summary>
    /// <returns></returns>
    private async Task RegisterJobs()
    {
        var quartzConfig = _configuration.GetSection("Quartz").Get<QuartzConfig>();
        ArgumentNullException.ThrowIfNull(quartzConfig, nameof(quartzConfig));
        foreach (QuartzJobConfig job in quartzConfig.Jobs)
        {
            var jobType = Type.GetType(job.JobType);
            ArgumentNullException.ThrowIfNull(jobType, nameof(jobType));
            IJobDetail jobDetail = JobBuilder.Create(jobType).WithIdentity(job.Name, job.Group).Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity($"{job.Name}-Trigger", job.Group).WithCronSchedule(job.CronExpression).Build();
            await _scheduler.ScheduleJob(jobDetail, trigger);
            _logger.LogInformation($"Scheduled Job: {job.Name} With Cron: {job.CronExpression}");
        }
    }
}
