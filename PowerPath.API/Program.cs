using PowerPath.CrossCut;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddBearerAuthentication(builder.Configuration, true);
builder.Services.AddServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication webApplication = builder.Build();

if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}

webApplication.UseHttpsRedirection();
webApplication.UseAuthentication();
webApplication.UseAuthorization();
webApplication.MapControllers();

webApplication.Run();
