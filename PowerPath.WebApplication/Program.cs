using PowerPath.Application.Facades;
using PowerPath.Application.Interfaces.Services;
using PowerPath.Application.Profiles;
using PowerPath.Application.Services;
using PowerPath.Domain.Interfaces.Facades.Repositories;
using PowerPath.Domain.Interfaces.Repositories;
using PowerPath.Domain.Interfaces.Services;
using PowerPath.Domain.Services;
using PowerPath.Infra.Files.Repositories;
using PowerPath.Infra.SQL.Contexts;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PowerPathContext>();

builder.Services.AddScoped<IMedidorSQLRepository, PowerPath.Infra.SQL.Repositories.MedidorRepository>();
builder.Services.AddScoped<IMedidorFileRepository, MedidorRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();

builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IMedidorService, MedidorService>();

builder.Services.AddScoped<IConsoleApplicationService, ConsoleApplicationService>();
builder.Services.AddScoped<IMedidorApplicationService, MedidorApplicationService>();
builder.Services.AddScoped<ILogApplicationService, LogApplicationService>();

builder.Services.AddScoped<IMedidorRepositoryFacade, MedidorRepositoryFacade>();

builder.Services.AddAutoMapper(typeof(MedidorProfile), typeof(LogProfile));

builder.Services.AddControllersWithViews();

WebApplication webApplication = builder.Build();

if (!webApplication.Environment.IsDevelopment())
{
    webApplication.UseExceptionHandler("/Home/Error");
    webApplication.UseHsts();
}

webApplication.UseHttpsRedirection();
webApplication.UseStaticFiles();
webApplication.UseRouting();
webApplication.UseAuthorization();

webApplication.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

webApplication.Run();
