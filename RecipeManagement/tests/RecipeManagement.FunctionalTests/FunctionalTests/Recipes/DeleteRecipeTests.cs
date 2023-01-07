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

public class DeleteRecipeTests : TestBase
{
    [Test]
    public async Task delete_recipe_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeRecipe);

        // Act
        var route = ApiRoutes.Recipes.Delete(fakeRecipe.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task delete_recipe_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());

        // Act
        var route = ApiRoutes.Recipes.Delete(fakeRecipe.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task delete_recipe_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.Recipes.Delete(fakeRecipe.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}