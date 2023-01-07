namespace RecipeManagement.Extensions.Services;

using RecipeManagement.Resources;
using RecipeManagement.Services;
using SharedKernel.Messages;
using MassTransit;
using RecipeManagement.Extensions.Services.ProducerRegistrations;
using RecipeManagement.Extensions.Services.ConsumerRegistrations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

public static class MassTransitServiceExtension
{
    public static void AddMassTransitServices(this IServiceCollection services, IWebHostEnvironment env)
    {
        if (!env.IsEnvironment(Consts.Testing.IntegrationTestingEnvName) 
            && !env.IsEnvironment(Consts.Testing.FunctionalTestingEnvName))
        {
            services.AddMassTransit(mt =>
            {
                mt.AddConsumers(Assembly.GetExecutingAssembly());
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(EnvironmentService.RmqHost, 
                        ushort.Parse(EnvironmentService.RmqPort), 
                        EnvironmentService.RmqVirtualHost, 
                        h =>
                        {
                            h.Username(EnvironmentService.RmqUsername);
                            h.Password(EnvironmentService.RmqPassword);
                        });

                    // Producers -- Do Not Delete This Comment
                    cfg.AddRecipeProducerEndpoint();

                    // Consumers -- Do Not Delete This Comment
                    cfg.AddToBookEndpoint(context);
                });
            });
            services.AddOptions<MassTransitHostOptions>();
        }
    }
}
