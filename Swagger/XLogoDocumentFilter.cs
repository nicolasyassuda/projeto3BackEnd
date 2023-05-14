using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace APIMOVIES.Swagger
{
    public class XLogoDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Info.Extensions = new Dictionary<string, Microsoft.OpenApi.Interfaces.IOpenApiExtension>
            {
                {
                    "x-logo", new Microsoft.OpenApi.Any.OpenApiObject
                    {
                        {"url", new Microsoft.OpenApi.Any.OpenApiString("https://www.smartbrain.com.br/hubfs/logo_branco_site.png")},
                        {"altText", new Microsoft.OpenApi.Any.OpenApiString("Smartbrain Logo")},
                        {"backgroundColor", new Microsoft.OpenApi.Any.OpenApiString("#373e42")},
                        {"height", new Microsoft.OpenApi.Any.OpenApiString("50px")},
                    }
                },
            };
        }
    }
}
