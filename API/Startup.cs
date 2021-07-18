using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application.Posts;
using API.Extensions;
using FluentValidation.AspNetCore;
using API.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Sets up dependency injection here.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                // Here we are specifying an app wide authentication policy that will make auth the required default unless otherwise specified.
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            }).AddFluentValidation(config => 
            {
                // Here we can register the classes where fluent validation is applied.
                // validation needs to be tied to the controllers as they format the response that
                // comes back.
                config.RegisterValidatorsFromAssemblyContaining<PostCreate>();
            });
            // DP Section.
            // configure main services... 
            services.AddApplicationServices(_config);
            // configured identity...
            services.AddIdentityServices(_config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // We can setup middleware here as the http request makes its way through the pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Register the exception middleware as the first middleware so that we detect errors right away
            // to stop processing an return the response.
            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                // Disabled as ExceptionMiddleware will handle error response.
                // app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();
            // We have configured this in IdentityServiceExtensions.
            app.UseAuthentication();
            // Authenticate before.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
