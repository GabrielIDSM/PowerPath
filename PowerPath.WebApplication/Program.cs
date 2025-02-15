using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = "PowerPath",
            ValidAudience = "Default",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY")!))
        };
    });

builder.Services.AddDbContext<PowerPathContext>();

builder.Services.AddScoped<IJWTSecurity, JWTSecurity>();
builder.Services.AddScoped<ISenhaSecurity, SenhaSecurity>();

builder.Services.AddScoped<IMedidorSQLRepository, PowerPath.Infra.SQL.Repositories.MedidorRepository>();
builder.Services.AddScoped<IMedidorFileRepository, PowerPath.Infra.Files.Repositories.MedidorRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IMedidorService, MedidorService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IConsoleApplicationService, ConsoleApplicationService>();
builder.Services.AddScoped<IMedidorApplicationService, MedidorApplicationService>();
builder.Services.AddScoped<ILogApplicationService, LogApplicationService>();
builder.Services.AddScoped<IUsuarioApplicationService, UsuarioApplicationService>();

builder.Services.AddScoped<IMedidorRepositoryFacade, MedidorRepositoryFacade>();

builder.Services.AddAutoMapper(typeof(MedidorProfile), typeof(LogProfile), typeof(UsuarioProfile));

builder.Services.AddControllersWithViews();

WebApplication webApplication = builder.Build();

if (!webApplication.Environment.IsDevelopment())
{
    webApplication.UseHsts();
}

webApplication.UseHttpsRedirection();
webApplication.UseStaticFiles();
webApplication.UseRouting();
webApplication.UseAuthentication();
webApplication.UseAuthorization();

webApplication.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

webApplication.Run();
