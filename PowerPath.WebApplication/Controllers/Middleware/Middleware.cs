namespace PowerPath.WebApplication.Controllers.Middleware
{
    public class Middleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            context.Items["Username"] = context.Request.Cookies["Username"];
            await _next(context);
        }
    }
}
