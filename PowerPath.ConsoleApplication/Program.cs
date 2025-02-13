using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using PowerPath.Application.Services;
using PowerPath.Domain.Services;
using PowerPath.Infra.SQL.Contexts;
using PowerPath.Domain.Interfaces.Services;
using PowerPath.Application.Interfaces.Services;
using PowerPath.Application.Facades;
using PowerPath.Application.Profiles;
using PowerPath.Domain.Interfaces.Facades.Repositories;
using PowerPath.Domain.Interfaces.Repositories;
using PowerPath.Infra.Files.Repositories;

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
            Console.WriteLine(_consoleAppService.ProcessarComando(input));
            Console.WriteLine("\n[PowerPath]\nDigite um comando, '?' para exibir ajuda ou 'x' para sair:");
        }
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                string environment = context.HostingEnvironment.EnvironmentName;

                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<PowerPathContext>();

                services.AddScoped<IMedidorSQLRepository, PowerPath.Infra.SQL.Repositories.MedidorRepository>();
                services.AddScoped<IMedidorFileRepository, MedidorRepository>();
                services.AddScoped<ILogRepository, LogRepository>();

                services.AddScoped<ILogService, LogService>();
                services.AddScoped<IMedidorService, MedidorService>();

                services.AddScoped<IConsoleApplicationService, ConsoleApplicationService>();
                services.AddScoped<IMedidorApplicationService, MedidorApplicationService>();
                services.AddScoped<ILogApplicationService, LogApplicationService>();

                services.AddScoped<IMedidorRepositoryFacade, MedidorRepositoryFacade>();

                services.AddAutoMapper(typeof(MedidorProfile), typeof(LogProfile));
            });
    } 
}
