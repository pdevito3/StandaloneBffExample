namespace RecipeManagement.Extensions.Application;

using RecipeManagement.Middleware;
using RecipeManagement.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerUI;

public static class SwaggerAppExtension
{
    public static void UseSwaggerExtension(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            config.SwaggerEndpoint("/swagger/v1/swagger.json", "");
            config.DocExpansion(DocExpansion.None);
            config.OAuthClientId(EnvironmentService.ClientId);
            config.OAuthClientSecret(EnvironmentService.ClientSecret);
            config.OAuthUsePkce();
        });
    }
}