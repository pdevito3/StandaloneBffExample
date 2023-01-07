namespace RecipeManagement.Extensions.Services;

using RecipeManagement.Databases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using HeimGuard;
using RecipeManagement.Services;
using RecipeManagement.Resources;
using RecipeManagement.Services;
using Microsoft.EntityFrameworkCore;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IWebHostEnvironment env)
    {
        // DbContext -- Do Not Delete
        var connectionString = EnvironmentService.DbConnectionString;
        if(string.IsNullOrWhiteSpace(connectionString))
        {
            // this makes local migrations easier to manage. feel free to refactor if desired.
            connectionString = env.IsDevelopment() 
                ? "Host=localhost;Port=56628;Database=dev_recipemanagement;Username=postgres;Password=postgres"
                : throw new Exception("DB_CONNECTION_STRING environment variable is not set.");
        }

        services.AddDbContext<RecipesDbContext>(options =>
            options.UseNpgsql(connectionString,
                builder => builder.MigrationsAssembly(typeof(RecipesDbContext).Assembly.FullName))
                            .UseSnakeCaseNamingConvention());

        services.AddHostedService<MigrationHostedService<RecipesDbContext>>();

        // Auth -- Do Not Delete
        if (!env.IsEnvironment(Consts.Testing.FunctionalTestingEnvName))
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = EnvironmentService.Authority;
                    options.Audience = EnvironmentService.Audience;
                    options.RequireHttpsMetadata = !env.IsDevelopment();
                });
        }

        services.AddAuthorization(options =>
        {
        });

        services.AddHeimGuard<UserPolicyHandler>()
            .MapAuthorizationPolicies()
            .AutomaticallyCheckPermissions();
    }
}
