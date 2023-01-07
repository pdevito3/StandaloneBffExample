namespace RecipeManagement.FunctionalTests.FunctionalTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateIngredientTests : TestBase
{
    [Test]
    public async Task create_ingredient_returns_created_using_valid_dto()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeIngredient = new FakeIngredientForCreationDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)
            .Generate();

        // Act
        var route = ApiRoutes.Ingredients.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeIngredient);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}