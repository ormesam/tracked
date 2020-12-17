using System;
using System.Net;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Api.Middleware {
    public class ExceptionLoggingMiddleware : IMiddleware {
        private ModelDataContext context;

        public ExceptionLoggingMiddleware(ModelDataContext context) {
            this.context = context;
        }


        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            try {
                await next(context);
            } catch (Exception ex) {
                await HandleExceptionAsync(context, ex);

                this.context.TraceMessages.Add(new TraceMessage {
                    DateUtc = DateTime.UtcNow,
                    Message = ex.ToString(),
                });

                this.context.SaveChanges();
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex) {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

#if DEBUG
            await context.Response.WriteAsync(ex.ToString());
#endif
        }
    }

    public static class ExceptionLoggingMiddlewareExtensions {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app) {
            app.UseMiddleware<ExceptionLoggingMiddleware>();
        }
    }
}
