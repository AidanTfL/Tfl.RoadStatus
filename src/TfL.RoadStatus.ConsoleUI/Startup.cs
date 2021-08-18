using System;
using FluentValidation;
using FluentValidation.AspNetCore;
using Flurl.Http.Configuration;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TfL.RoadStatus.Application;
using TfL.RoadStatus.Application.GetRoadStatus;
using TfL.RoadStatus.Application.Interfaces;
using TfL.RoadStatus.ConsoleUI.Filters;
using TfL.RoadStatus.Infrastructure;

namespace TfL.RoadStatus.ConsoleUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppConfig>(Configuration); //Adds IOptions<AppConfig> to services

            services.AddHostedService<RoadStatusWorker>();

            services.AddLogging(l =>
                l.AddConfiguration(Configuration.GetSection(nameof(AppConfig.Logging))));

            services.AddMediatR(typeof(GetRoadStatusQuery));

            services.AddFluentValidation()
                .AddTransient<IValidator<GetRoadStatusQuery>, GetRoadStatusQueryValidator>();

            services.AddAutoMapper(c =>
                c.AddProfile<RoadStatusProfile>());

            services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
            services.AddScoped<IRoadClient, TflRoadClient>();
            services.AddTransient<IUrlBuilder, TflUrlBuilder>();

            // Add custom exception filter handler
            AppDomain.CurrentDomain.UnhandledException += ConsoleExceptionFilter.HandleException;
        }

        public void Configure(IConfigurationBuilder app, IHostEnvironment env, ConsoleArgs args)
        {
            app.AddEnvironmentVariables();
            app.AddUserSecrets<Program>();
            app.AddJsonFile("appsettings.json", false);
            app.AddJsonStream(args
                .ToJsonStream()); //unlike .AddCommandLineArgs, maps both parameter (i.e: --appKey) and value args (i.e: roadIds)
        }
    }
}