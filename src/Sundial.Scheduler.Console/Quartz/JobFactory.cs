using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Sundial.Scheduler.Quartz;

/// <summary>
/// 
/// </summary>
public class JobFactory : IJobFactory
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="provider"></param>
    public JobFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="scheduler"></param>
    /// <returns></returns>
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return (IJob)_provider.GetRequiredService(bundle.JobDetail.JobType);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="job"></param>
    public void ReturnJob(IJob job)
    {
        (job as IDisposable)?.Dispose();
    }
}
