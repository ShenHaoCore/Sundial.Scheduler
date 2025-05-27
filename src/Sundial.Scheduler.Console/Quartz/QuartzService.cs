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
}
