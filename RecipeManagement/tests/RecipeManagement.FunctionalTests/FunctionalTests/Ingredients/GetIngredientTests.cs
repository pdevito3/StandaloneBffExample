namespace RecipeManagement.FunctionalTests.FunctionalTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetIngredientTests : TestBase
{
    [Test]
    public async Task get_ingredient_returns_success_when_entity_exists()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeIngredient = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id).Generate());
        await InsertAsync(fakeIngredient);

        // Act
        var route = ApiRoutes.Ingredients.GetRecord(fakeIngredient.Id);
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}