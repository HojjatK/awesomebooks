using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using AwesomeBooks.Domain.EF;
using AwesomeBooks.Domain.Entities;

namespace AwesomeBooks.Web.Middlewares
{
    public class EnsureDatabaseCreatedMiddleware
    {
        private readonly IServiceProvider _serviceProvider;        
        private readonly RequestDelegate _next;

        public EnsureDatabaseCreatedMiddleware(
            IServiceProvider serviceProvider,
            RequestDelegate next)
        {
            _serviceProvider = serviceProvider;            
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            bool installRequired = false;
            using (var scope = _serviceProvider.CreateScope())
            {
                var domainContext = scope.ServiceProvider.GetService<DomainContext>();
                domainContext.Database.EnsureCreated();

                var appSettings = domainContext.AppSettings.FirstOrDefault();
                if (appSettings == null || !appSettings.Installed)
                {
                    installRequired = true;
                }
            }

            if (!installRequired)
            {
                // Call the next delegate/middleware in the pipeline
                return this._next(context);
            }            
            string requestPath = context.Request.Path.Value != null
                                ? context.Request.Path.Value.Trim().ToLower()
                                : "";

            bool isSetupRequest = requestPath.StartsWith("/setup", StringComparison.OrdinalIgnoreCase);
            bool isBrowserLink = requestPath.StartsWith("/__browserlink");
            if (!isBrowserLink && !isSetupRequest)
            {
                context.Response.Redirect("/setup");
                return Task.FromResult(0);
            }

            return this._next(context);
        }
    }

    public static class EnsureDatabaseCreatedMiddlewareExtensions
    {
        public static IApplicationBuilder UseInstaller(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<EnsureDatabaseCreatedMiddleware>();
        }
    }
}
