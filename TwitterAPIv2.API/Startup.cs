using Common.Presentation;
using MediatR;
using Microsoft.Extensions.Options;
using Serilog;
using TwitterAPIv2.API.StartupExtensions;
using TwitterAPIv2.Application.Handlers;
using TwitterAPIv2.Application.Queries;
using TwitterAPIv2.Infrastructure.Services;
using TwitterAPIv2.Infrastructure.Utilities;

namespace TwitterAPIv2.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "v1");
                //c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(typeof(GetSampleTweetsQuery).Assembly);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.ConfigureSwagger();

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel
                .Information()
                .WriteTo.Console()
                .Enrich.WithCorrelationIdHeader()
                .Enrich.WithCorrelationId();

            Log.Logger = loggerConfiguration.CreateLogger();

            services.AddScoped<ITwitterService, TwitterService>();
            services.AddScoped<ITwitterAuthApi, TwitterAuthApi>();
            services.AddScoped<ICalculateMetrics, CalculateMetrics>();

            services.AddTwitterApi(Configuration);
            services.AddSingleton(ExtractTwitterApiConfiguration());
        }

        private IOptions<TwitterAuthSettings> ExtractTwitterApiConfiguration()
        {
            var twitterAuth = new TwitterAuthSettings
            {
                BaseUrl = Configuration.TryGetStringValue("TwitterAuth__BaseUrl"),
                AuthUrl = Configuration.TryGetStringValue("TwitterAuth__AuthUrl"),
                ClientId = Configuration.TryGetStringValue("TwitterAuth__ClientId"),
                ClientSecret = Configuration.TryGetStringValue("TwitterAuth__ClientSecret"),
                BearerToken = Configuration.TryGetStringValue("TwitterAuth__BearerToken")
            };
            var options = Options.Create(twitterAuth);
            return options;
        }
    }
}
