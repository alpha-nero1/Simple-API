using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using System.Net;
using Application.Core;
using System.Text.Json;


namespace API.Middleware
{
    // This is a class that will intercept any exceptions that coccur within the app and
    // format and return a standard/safe exception response. We configure this in the Startup.cs.
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env
        )
        {
            _env = env;
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // What we are doing here is saying go on and execute the application logic
                // (passing down to mediator then handler e.t.c.). If we run into any exceptions,
                // we catch it and we have an opportunity to catch and return a standard Exception response.
                // This also ensures that we catch exceptions at the earliest possible point in the
                // execution of business logic.
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                bool isDev = _env.IsDevelopment();
                AppException errResponse;
                if (isDev) errResponse = new AppException(context.Response.StatusCode, e.Message, e.StackTrace.ToString());
                else errResponse = new AppException(context.Response.StatusCode, "Server error");

                var jsonOpts = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var json = JsonSerializer.Serialize(errResponse, jsonOpts);
                // Here in the case of error we have written the JSON response manually onto the return
                // api response instead of allowing dotnet to handle it automatically.
                await context.Response.WriteAsync(json);
            }
        }
    }
}