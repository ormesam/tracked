using System;
using System.Net;
using System.Threading.Tasks;
using Api.Utility;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Api.Middleware {
    public class ExceptionLoggingMiddleware : IMiddleware {
        private DbFactory dbFactory;

        public ExceptionLoggingMiddleware(DbFactory dbFactory) {
            this.dbFactory = dbFactory;
        }


        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            try {
                await next(context);
            } catch (Exception ex) {
                await HandleExceptionAsync(context, ex);

                using (Transaction transaction = dbFactory.CreateTransaction()) {
                    using (ModelDataContext dbContext = transaction.CreateDataContext()) {
                        dbContext.TraceMessages.Add(new TraceMessage {
                            DateUtc = DateTime.UtcNow,
                            Message = ex.ToString(),
                        });

                        dbContext.SaveChanges();
                    }

                    transaction.Commit();
                }
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex) {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

#if DEBUG
            return context.Response.WriteAsync(ex.ToString());
#else
            return Task.CompletedTask;
#endif
        }
    }

    public static class ExceptionLoggingMiddlewareExtensions {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app) {
            app.UseMiddleware<ExceptionLoggingMiddleware>();
        }
    }
}
