using PowerPath.CrossCut;
using PowerPath.WebApplication.Controllers.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddBearerAuthentication(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

builder.Services.AddControllersWithViews();

WebApplication webApplication = builder.Build();

if (!webApplication.Environment.IsDevelopment())
{
    webApplication.UseHsts();
}

webApplication.UseHttpsRedirection();
webApplication.UseStaticFiles();
webApplication.UseRouting();
webApplication.UseMiddleware<Middleware>();
webApplication.UseAuthentication();
webApplication.UseAuthorization();

webApplication.MapControllerRoute(
    name: "default",
    pattern: "{controller=Medidor}/{action=Index}");

webApplication.Run();
