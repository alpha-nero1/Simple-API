using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using AutoMapper;
using MediatR;
using Application.Posts;
using Application.Core;
using Application.Interfaces;
using Infrastructure.Security;

namespace API.Extensions
{
    // Return the collection of services for dependency injection.
    // Effectively setting up dependency injection here.
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            services.AddDbContext<DataContext>(opts =>
            {
                opts.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            // Add mediator and specify location of handlers.
            services.AddMediatR(typeof(PostList).Assembly);
            // Add automapper and specify location of profiles.
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddScoped<IUserAccessor, UserAccessor>();
            return services;
        }
    }
}