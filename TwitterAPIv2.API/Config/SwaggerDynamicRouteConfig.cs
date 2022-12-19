using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TwitterAPIv2.API.Config
{
    /// <summary>
    /// Configuration for Swagger documents routing by API version
    /// </summary>
    public class SwaggerDynamicRouteConfig : IDocumentFilter
    {
        /// <summary>
        /// Applies API version to Swagger documents.
        /// </summary>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                paths.Add(
                    path.Key.Replace("v{version}", swaggerDoc.Info.Version),
                    path.Value
                );
            }

            swaggerDoc.Paths = paths;
        }
    }
}
