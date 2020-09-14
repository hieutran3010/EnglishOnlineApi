#region

using System;
using AuthService.Firebase.Firebase;
using BackgroundTaskQueueNet;
using BackgroundTaskQueueNet.Abstracts;
using EFPostgresEngagement.Abstract;
using EFPostgresEngagement.Extensions;
using GraphQLDoorNet.Abstracts;
using GraphQLDoorNet.Extensions;
using HelenExpress.Data;
using HelenExpress.Data.MiddleLayers;
using HelenExpress.GraphQL.HostedServices;
using HelenExpress.GraphQL.HostedServices.ExportBill;
using HelenExpress.GraphQL.Infrastructure.ModelMapping;
using HelenExpress.GraphQL.Schema;
using HelenExpress.GraphQL.Services;
using HelenExpress.GraphQL.Services.Abstracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IUnitOfWork = GraphQLDoorNet.Abstracts.IUnitOfWork;

#endregion

namespace HelenExpress.GraphQL
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
                .UsePostgresSql<HeLenExpressDbContext>(Configuration)
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
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<IInputMapper, InputMapper>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<IFileService, FileService>();

            ModelMapping.Mapping();

            this.RegisterHostedServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UpdateDatabase<HeLenExpressDbContext>();

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
            services.AddSingleton(typeof(IBackgroundTaskQueue<>), typeof(BackgroundTaskQueue<>));
            services.AddHostedService(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<BillExportHostedService>>();
                var taskQueue = provider.GetRequiredService<IBackgroundTaskQueue<BillExportTaskContext>>();
                return new BillExportHostedService(taskQueue, logger);
            });
            // services.AddHostedService<CacheBillQuotation>();
            // services.AddHostedService<CorrectCustomerData>();
            // services.AddHostedService<SupportUnicodeSearch>();
        }
    }
}