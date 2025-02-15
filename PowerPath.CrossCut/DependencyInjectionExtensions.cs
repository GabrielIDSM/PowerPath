using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

namespace PowerPath.CrossCut
{
    public static class DependencyInjectionExtensions
    {
        public static void AddBearerAuthentication(this IServiceCollection services, IConfiguration configuration, bool isAPI = false)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    if (isAPI)
                    {
                        options.Events = new JwtBearerEvents
                        {
                            OnChallenge = context =>
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                context.Response.ContentType = "application/json";
                                return context.Response.WriteAsync("{\"error\": \"O token informado é inválido ou está expirado.\"}");
                            },
                            OnMessageReceived = context =>
                            {
                                var autorizacao = context.Request.Headers.Authorization.FirstOrDefault();
                                if (!string.IsNullOrEmpty(autorizacao) && autorizacao.StartsWith("Bearer "))
                                {
                                    context.Token = autorizacao["Bearer ".Length..].Trim();
                                }
                                return Task.CompletedTask;
                            }
                        };
                    }
                    else
                    {
                        options.Events = new JwtBearerEvents
                        {
                            OnChallenge = context =>
                            {
                                context.HandleResponse();
                                context.Response.Redirect("/Login/Index");
                                return Task.CompletedTask;
                            },
                            OnMessageReceived = context =>
                            {
                                string? token = context.Request.Cookies["AuthToken"];
                                if (!string.IsNullOrEmpty(token))
                                {
                                    context.Token = token;
                                }
                                return Task.CompletedTask;
                            }
                        };
                    }

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
        }

        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
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
        }
    }
}
