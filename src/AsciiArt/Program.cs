using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AsciiArt.Services;

namespace AsciiArt;

public static class Program 
{
    public static void Main() 
    {
        var host = Host.CreateDefaultBuilder()
        .ConfigureServices(ConfigureServices)
        .Build();

        var executor = host.Services.GetRequiredService<Executor>();
        executor.Execute();
    }

    private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services
        .AddSingleton<ICoreService,CoreService>()
        .AddSingleton<IDonutService,DonutService>()
        .AddSingleton<Executor>();
    }
}

public class Executor
{
    private readonly ICoreService _coreService;

    public Executor(ICoreService coreService)
    {
        _coreService = coreService;
    }

    public void Execute()
    {
        _coreService.ExecuteDonut();
    }
}

