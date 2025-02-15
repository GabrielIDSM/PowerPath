using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerPath.Application.Facades;
using PowerPath.Application.Interfaces.Services;
using PowerPath.Application.Profiles;
using PowerPath.Application.Services;
using PowerPath.Domain.Interfaces.Facades.Repositories;
using PowerPath.Domain.Interfaces.Repositories;
using PowerPath.Domain.Interfaces.Repositories.Medidores;
using PowerPath.Domain.Interfaces.Security;
using PowerPath.Domain.Interfaces.Services;
using PowerPath.Domain.Services;
using PowerPath.Infra.Files.Repositories;
using PowerPath.Infra.Security;
using PowerPath.Infra.SQL.Contexts;
using PowerPath.Infra.SQL.Repositories;

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
                services.AddDbContext<PowerPathContext>();

                services.AddScoped<IJWTSecurity, JWTSecurity>();
                services.AddScoped<ISenhaSecurity, SenhaSecurity>();

                services.AddScoped<IMedidorSQLRepository, PowerPath.Infra.SQL.Repositories.MedidorRepository>();
                services.AddScoped<IMedidorFileRepository, PowerPath.Infra.Files.Repositories.MedidorRepository>();
                services.AddScoped<ILogRepository, LogRepository>();
                services.AddScoped<IUsuarioRepository, UsuarioRepository>();

                services.AddScoped<ILogService, LogService>();
                services.AddScoped<IMedidorService, MedidorService>();
                services.AddScoped<IUsuarioService, UsuarioService>();

                services.AddScoped<IConsoleApplicationService, ConsoleApplicationService>();
                services.AddScoped<IMedidorApplicationService, MedidorApplicationService>();
                services.AddScoped<ILogApplicationService, LogApplicationService>();
                services.AddScoped<IUsuarioApplicationService, UsuarioApplicationService>();

                services.AddScoped<IMedidorRepositoryFacade, MedidorRepositoryFacade>();

                services.AddAutoMapper(typeof(MedidorProfile), typeof(LogProfile), typeof(UsuarioProfile));
            });
    }
}
