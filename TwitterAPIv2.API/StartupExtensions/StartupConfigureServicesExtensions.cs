using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using CorrelationId.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TwitterAPIv2.API.Config;

namespace TwitterAPIv2.API.StartupExtensions
{
    /// <summary>
    /// Extensions for startup services configuration
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class StartupConfigureServicesExtensions
    {
        /// <summary>
        /// Configures the auto generated Swagger OpenAPI spec documentation
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Version 1", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                c.AddSecurityDefinition("Bearer", StartupConfigureServicesExtensions.GetSwaggerSecurityDefinition());
                c.AddSecurityRequirement(StartupConfigureServicesExtensions.GetSwaggerSecurityRequiement());

                c.DocumentFilter<SwaggerDynamicRouteConfig>();

                c.IncludeXmlComments(StartupConfigureServicesExtensions.GetSwaggerXMLPath());
            });
        }


        /// <summary>
        /// Returns the SecurityRequirement for SwaggerConfiguration
        /// </summary>
        [ExcludeFromCodeCoverage]
        private static OpenApiSecurityRequirement GetSwaggerSecurityRequiement()
        {
            return new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                };
        }

        /// <summary>
        /// Returns the SecurityScheme for SwaggerConfiguration
        /// </summary>
        private static OpenApiSecurityScheme GetSwaggerSecurityDefinition()
        {
            return new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                          Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                };
        }

        /// <summary>
        /// Returns the xmlPath for SwaggerConfiguration
        /// </summary>
        private static string GetSwaggerXMLPath()
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            return Path.Combine(AppContext.BaseDirectory, xmlFile);
        }
        
        /// <summary>
        /// Configures correlationId header
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static void ConfigureCorrelationId(this IServiceCollection services)
        {
            services.AddDefaultCorrelationId(options =>
            {
                options.RequestHeader = "x-correlation-id";
                options.ResponseHeader = "x-correlation-id";
                options.UpdateTraceIdentifier = true;
            });

            // Needed for the ILogger to access the TraceId / CorrelationId
            services.AddHttpContextAccessor();
        }
    }
}
