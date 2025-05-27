using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using Serilog;
using Sundial.Scheduler.Jobs;
using Sundial.Scheduler.Models;
using Sundial.Scheduler.Quartz;
using Topshelf;

var configuration = new ConfigurationBuilder().AddInMemoryCollection().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).Enrich.FromLogContext().CreateLogger();

ServiceCollection services = new ServiceCollection();
services.AddLogging(builder => builder.AddSerilog(dispose: true));
services.AddSingleton<IConfiguration>(configuration);
services.AddTransient<QuartzService>();
services.AddScoped<IJobFactory, JobFactory>();
services.AddScoped<TestJob>();
services.AddScoped<BilibiliJob>();
ServiceProvider serviceProvider = services.BuildServiceProvider();

var appConfig = configuration.GetSection("App").Get<AppConfig>();
ArgumentNullException.ThrowIfNull(appConfig, nameof(appConfig));

TopshelfExitCode rc = HostFactory.Run(x =>
{
    x.UseSerilog();
    x.Service<QuartzService>(s => {
        s.ConstructUsing(() => serviceProvider.GetRequiredService<QuartzService>());
        s.WhenStarted(async tc => await tc.StartAsync());
        s.WhenStopped(async tc => await tc.StopAsync());
    });
    x.SetServiceName(appConfig.ServiceName); // 服务名称（唯一标识）
    x.SetDisplayName(appConfig.DisplayName); // 服务显示名称
    x.SetDescription(appConfig.Description); // 服务描述
    x.RunAsLocalSystem(); // 以本地系统账户运行
    x.StartAutomatically(); // 自动启动
    x.EnableServiceRecovery(r => {
        r.RestartService(1);  // 服务崩溃后 1 分钟后自动重启
        r.OnCrashOnly();  // 仅在崩溃时触发恢复
    });
});

int exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
Environment.ExitCode = exitCode;