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

public class CreateRecipeTests : TestBase
{
    [Test]
    public async Task create_recipe_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var fakeRecipe = new FakeRecipeForCreationDto().Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);

        // Act
        var route = ApiRoutes.Recipes.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeRecipe);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_recipe_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeRecipe = new FakeRecipeForCreationDto { }.Generate();

        // Act
        var route = ApiRoutes.Recipes.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeRecipe);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task create_recipe_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeRecipe = new FakeRecipeForCreationDto { }.Generate();
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.Recipes.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeRecipe);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}