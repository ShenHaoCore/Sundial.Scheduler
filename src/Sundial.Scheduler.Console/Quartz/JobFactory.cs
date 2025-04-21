using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Sundial.Scheduler.Quartz;

/// <summary>
/// 
/// </summary>
public class JobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    public JobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="scheduler"></param>
    /// <returns></returns>
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return (IJob)_serviceProvider.GetRequiredService(bundle.JobDetail.JobType);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="job"></param>
    public void ReturnJob(IJob job)
    {
    }
}
