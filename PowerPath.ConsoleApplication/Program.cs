using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerPath.Application.Interfaces.Services;
using PowerPath.CrossCut;

class Program
{
    static void Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();
        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider services = serviceScope.ServiceProvider;

        IConsoleApplicationService _consoleAppService = services.GetRequiredService<IConsoleApplicationService>();

        Console.WriteLine("[PowerPath]\nDigite um comando, '?' para exibir ajuda ou 'x' para sair:");
        string? input;
        while ((input = Console.ReadLine())?.ToLower() != "x")
        {
            _consoleAppService.ProcessarComando(input);
            Console.WriteLine("\n[PowerPath]\nDigite um comando, '?' para exibir ajuda ou 'x' para sair:");
        }
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                string environment = context.HostingEnvironment.EnvironmentName;

                config.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddServices(context.Configuration);
            });
    }
}
