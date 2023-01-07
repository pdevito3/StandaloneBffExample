namespace RecipeManagement.FunctionalTests.FunctionalTests.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.Domain;
using SharedKernel.Domain;
using RecipeManagement.SharedTestHelpers.Fakes.Author;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetRecipeTests : TestBase
{
    [Test]
    public async Task get_recipe_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeRecipe);

        // Act
        var route = ApiRoutes.Recipes.GetRecord(fakeRecipe.Id);
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_recipe_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());

        // Act
        var route = ApiRoutes.Recipes.GetRecord(fakeRecipe.Id);
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_recipe_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.Recipes.GetRecord(fakeRecipe.Id);
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}