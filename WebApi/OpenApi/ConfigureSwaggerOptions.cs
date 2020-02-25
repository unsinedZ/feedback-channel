using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.OpenApi
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider apiDescriptionProvider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiDescriptionProvider)
        {
            this.apiDescriptionProvider = apiDescriptionProvider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in apiDescriptionProvider.ApiVersionDescriptions)
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Feedback API",
                Version = description.ApiVersion.ToString(),
                Description = "Feedback application.",
            };

            if (description.IsDeprecated)
                info.Description += " This API version has been deprecated.";

            return info;
        }
    }
}