using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Reitti.Web.Service;
using System.Text;
using System.Text.Json;

namespace Reitti.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSingleton<IRouteService, RouteService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/api/stops", async context =>
                {
                    var routeService = context.RequestServices.GetService<IRouteService>();
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(routeService.GetStops()));
                });

                endpoints.MapGet("/api/route", async context =>
                {
                    var routeService = context.RequestServices.GetService<IRouteService>();
                    var route = routeService.GetRoute(context.Request.Query["from"], context.Request.Query["to"]);
                    if (route != null)
                    {
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(route, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), Encoding.UTF8);
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                    }
                });
            });
        }
    }
}
