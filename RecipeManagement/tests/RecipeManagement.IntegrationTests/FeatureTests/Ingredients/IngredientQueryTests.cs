namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class IngredientQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_ingredient_with_accurate_props()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id).Generate());
        await InsertAsync(fakeIngredientOne);

        // Act
        var query = new GetIngredient.Query(fakeIngredientOne.Id);
        var ingredient = await SendAsync(query);

        // Assert
        ingredient.Name.Should().Be(fakeIngredientOne.Name);
        ingredient.Quantity.Should().Be(fakeIngredientOne.Quantity);
        ingredient.ExpiresOn.Should().BeCloseTo((DateTime)fakeIngredientOne.ExpiresOn, 1.Seconds());
        ingredient.Measure.Should().Be(fakeIngredientOne.Measure);
        ingredient.RecipeId.Should().Be(fakeIngredientOne.RecipeId);
    }

    [Test]
    public async Task get_ingredient_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetIngredient.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}