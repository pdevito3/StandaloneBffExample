namespace RecipeManagement.IntegrationTests;

using RecipeManagement.Extensions.Services;
using MassTransit.Testing;
using MassTransit;
using RecipeManagement.Domain.Recipes.Features;
using RecipeManagement.Databases;
using RecipeManagement;
using RecipeManagement.Services;
using RecipeManagement.Resources;
using RecipeManagement.Services;
using HeimGuard;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Npgsql;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Abstractions;
using DotNet.Testcontainers.Containers.Modules.Databases;
using FluentAssertions;
using FluentAssertions.Extensions;

[SetUpFixture]
public class TestFixture
{
    private static IServiceScopeFactory _scopeFactory;
    private static ServiceProvider _provider;
    private static InMemoryTestHarness _harness;
    private readonly TestcontainerDatabase _dbContainer = dbSetup();

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        await _dbContainer.StartAsync();
        Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", _dbContainer.ConnectionString);
        
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
        {
            EnvironmentName = Consts.Testing.IntegrationTestingEnvName,
        });
        builder.Configuration.AddEnvironmentVariables();

        builder.ConfigureServices();
        var services = builder.Services;

        // add any mock services here
        services.ReplaceServiceWithSingletonMock<IHttpContextAccessor>();
        services.ReplaceServiceWithSingletonMock<IHeimGuardClient>();

        // MassTransit Harness Setup -- Do Not Delete Comment
        services.AddMassTransitInMemoryTestHarness(cfg =>
        {
            cfg.AddMassTransitTestHarness(harness => 
            {
                // Consumer Registration -- Do Not Delete Comment

                harness.AddConsumer<AddToBook>();
            });
        });

        _provider = services.BuildServiceProvider();
        _scopeFactory = _provider.GetService<IServiceScopeFactory>();

        // MassTransit Start Setup -- Do Not Delete Comment
        _harness = _provider.GetRequiredService<InMemoryTestHarness>();
        await _harness.Start();


        SetupDateAssertions();
        await EnsureDatabase();
        await ResetState();
    }

    private static async Task EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<RecipesDbContext>();

        await context?.Database?.MigrateAsync();
    }

    public static TScopedService GetService<TScopedService>()
    {
        var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetService<TScopedService>();
        return service;
    }


    public static void SetUserRole(string role, string sub = null)
    {
        sub ??= Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, sub)
        };

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = Mock.Of<HttpContext>(c => c.User == claimsPrincipal);

        var httpContextAccessor = GetService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = httpContext;
    }

    public static void SetUserRoles(string[] roles, string sub = null)
    {
        sub ??= Guid.NewGuid().ToString();
        var claims = new List<Claim>();
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        claims.Add(new Claim(ClaimTypes.Name, sub));

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = Mock.Of<HttpContext>(c => c.User == claimsPrincipal);

        var httpContextAccessor = GetService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = httpContext;
    }
    
    public static void SetMachineRole(string role, string clientId = null)
    {
        clientId ??= Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim("client_role", role),
            new Claim("client_id", clientId)
        };

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = Mock.Of<HttpContext>(c => c.User == claimsPrincipal);

        var httpContextAccessor = GetService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = httpContext;
    }

    public static void SetMachineRoles(string[] roles, string clientId = null)
    {
        clientId ??= Guid.NewGuid().ToString();
        var claims = new List<Claim>();
        foreach (var role in roles)
        {
            claims.Add(new Claim("client_role", role));
        }
        claims.Add(new Claim("client_id", clientId));

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = Mock.Of<HttpContext>(c => c.User == claimsPrincipal);

        var httpContextAccessor = GetService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = httpContext;
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task ResetState()
    {
        await using var connection = new NpgsqlConnection(EnvironmentService.DbConnectionString);
        await connection.OpenAsync();
        try
        {
            var respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                TablesToIgnore = new Table[] { "__EFMigrationsHistory" },
                SchemasToExclude = new[] { "information_schema", "pg_subscription", "pg_catalog", "pg_toast" },
                DbAdapter = DbAdapter.Postgres
            });
            await respawner.ResetAsync(connection);
        }
        catch (InvalidOperationException e)
        {
            throw new Exception($"There was an issue resetting your database state. You might need to add a migration to your project. You can add a migration with `dotnet ef migration add YourMigrationDescription`. More details on this error: {e.Message}");
        }
    }

    public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<RecipesDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<RecipesDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RecipesDbContext>();

        try
        {
            //await dbContext.BeginTransactionAsync();

            await action(scope.ServiceProvider);

            //await dbContext.CommitTransactionAsync();
        }
        catch (Exception)
        {
            //dbContext.RollbackTransaction();
            throw;
        }
    }

    public static async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RecipesDbContext>();

        try
        {
            //await dbContext.BeginTransactionAsync();

            var result = await action(scope.ServiceProvider);

            //await dbContext.CommitTransactionAsync();

            return result;
        }
        catch (Exception)
        {
            //dbContext.RollbackTransaction();
            throw;
        }
    }

    public static Task ExecuteDbContextAsync(Func<RecipesDbContext, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<RecipesDbContext>()));

    public static Task ExecuteDbContextAsync(Func<RecipesDbContext, ValueTask> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<RecipesDbContext>()).AsTask());

    public static Task ExecuteDbContextAsync(Func<RecipesDbContext, IMediator, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<RecipesDbContext>(), sp.GetService<IMediator>()));

    public static Task<T> ExecuteDbContextAsync<T>(Func<RecipesDbContext, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<RecipesDbContext>()));

    public static Task<T> ExecuteDbContextAsync<T>(Func<RecipesDbContext, ValueTask<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<RecipesDbContext>()).AsTask());

    public static Task<T> ExecuteDbContextAsync<T>(Func<RecipesDbContext, IMediator, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<RecipesDbContext>(), sp.GetService<IMediator>()));

    public static Task<int> InsertAsync<T>(params T[] entities) where T : class
    {
        return ExecuteDbContextAsync(db =>
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Add(entity);
            }
            return db.SaveChangesAsync();
        });
    }

    // MassTransit Methods -- Do Not Delete Comment
    /// <summary>
    /// Publishes a message to the bus, and waits for the specified response.
    /// </summary>
    /// <param name="message">The message that should be published.</param>
    /// <typeparam name="TMessage">The message that should be published.</typeparam>
    public static async Task PublishMessage<TMessage>(object message)
        where TMessage : class
    {
        await _harness.Bus.Publish<TMessage>(message);
    }
    
    /// <summary>
    /// Confirm that a message has been published for this harness.
    /// </summary>
    /// <typeparam name="TMessage">The message that should be published.</typeparam>
    /// <returns>A boolean of true if a message of the given type has been published.</returns>
    public static async Task<bool> IsPublished<TMessage>()
        where TMessage : class
    {
        return await _harness.Published.Any<TMessage>();
    }
    
    /// <summary>
    /// Confirm that a message has been consumed for this harness.
    /// </summary>
    /// <typeparam name="TMessage">The message that should be consumed.</typeparam>
    /// <returns>A boolean of true if a message of the given type has been consumed.</returns>
    public static async Task<bool> IsConsumed<TMessage>()
        where TMessage : class
    {
        return await _harness.Consumed.Any<TMessage>();
    }
    
    /// <summary>
    /// The desired consumer consumed the message.
    /// </summary>
    /// <typeparam name="TMessage">The message that should be consumed.</typeparam>
    /// <typeparam name="TConsumedBy">The consumer of the message.</typeparam>
    /// <returns>A boolean of true if a message of the given type has been consumed by the given consumer.</returns>
    public static async Task<bool> IsConsumed<TMessage, TConsumedBy>()
        where TMessage : class
        where TConsumedBy : class, IConsumer
    {
        var consumerHarness = _provider.GetRequiredService<IConsumerTestHarness<TConsumedBy>>();
        return await consumerHarness.Consumed.Any<TMessage>();
    }

    private static TestcontainerDatabase dbSetup()
    {
        return new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "db",
                Username = "postgres",
                Password = "postgres"
            })
            .WithName($"IntegrationTesting_RecipeManagement_{Guid.NewGuid()}")
            .WithImage("postgres:latest")
            .Build();
    }

    private static void SetupDateAssertions()
    {
        // close to equivalency required to reconcile precision differences between EF and Postgres
        AssertionOptions.AssertEquivalencyUsing(options =>
        {
            options.Using<DateTime>(ctx => ctx.Subject
                .Should()
                .BeCloseTo(ctx.Expectation, 1.Seconds())).WhenTypeIs<DateTime>();
            options.Using<DateTimeOffset>(ctx => ctx.Subject
                .Should()
                .BeCloseTo(ctx.Expectation, 1.Seconds())).WhenTypeIs<DateTimeOffset>();

            return options;
        });
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await _dbContainer.DisposeAsync();
        
        // MassTransit Teardown -- Do Not Delete Comment
        await _harness.Stop();
    }
}



public static class ServiceCollectionServiceExtensions
{
    public static IServiceCollection ReplaceServiceWithSingletonMock<TService>(this IServiceCollection services)
        where TService : class
    {
        services.RemoveAll(typeof(TService));
        services.AddSingleton(_ => Mock.Of<TService>());
        return services;
    }
}
