#region

using System;
using AuthService.Firebase.Firebase;
using EFPostgresEngagement.Abstract;
using EFPostgresEngagement.Extensions;
using EnglishZone.Data;
using EnglishZone.Data.MiddleLayers;
using EnglishZone.GraphQL.Infrastructure.ModelMapping;
using EnglishZone.GraphQL.Schema;
using EnglishZone.GraphQL.Services;
using GraphQLDoorNet.Abstracts;
using GraphQLDoorNet.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IUnitOfWork = GraphQLDoorNet.Abstracts.IUnitOfWork;

#endregion

namespace EnglishZone.GraphQL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSentry(options =>
                {
                    options.MinimumEventLevel = LogLevel.Error;
                    options.Dsn = Environment.GetEnvironmentVariable("SENTRY_DSN");
                }));

            services.AddControllers();

            services
                .UsePostgresSql<EnglishZoneDbContext>(Configuration)
                .AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddGraphQLServices<Query, Mutation>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddFirebaseAuthService(Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID"));

            services.AddHttpContextAccessor();

            services.AddScoped<IDbTracker, UserProvider>();
            services.AddScoped<IInputMapper, InputMapper>();

            ModelMapping.Mapping();

            this.RegisterHostedServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UpdateDatabase<EnglishZoneDbContext>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            loggerFactory.AddSentry(options =>
            {
                options.MinimumEventLevel = LogLevel.Error;
                options.Dsn = Environment.GetEnvironmentVariable("SENTRY_DSN");
            });
        }

        private void RegisterHostedServices(IServiceCollection services)
        {
        }
    }
}