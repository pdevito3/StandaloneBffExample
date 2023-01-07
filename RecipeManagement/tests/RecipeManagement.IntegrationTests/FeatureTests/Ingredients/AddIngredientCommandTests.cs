namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using RecipeManagement.Domain.Ingredients.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class AddIngredientCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_ingredient_to_db()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeIngredientOne = new FakeIngredientForCreationDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id).Generate();

        // Act
        var command = new AddIngredient.Command(fakeIngredientOne);
        var ingredientReturned = await SendAsync(command);
        var ingredientCreated = await ExecuteDbContextAsync(db => db.Ingredients
            .FirstOrDefaultAsync(i => i.Id == ingredientReturned.Id));

        // Assert
        ingredientReturned.Name.Should().Be(fakeIngredientOne.Name);
        ingredientReturned.Quantity.Should().Be(fakeIngredientOne.Quantity);
        ingredientReturned.ExpiresOn.Should().BeCloseTo((DateTime)fakeIngredientOne.ExpiresOn, 1.Seconds());
        ingredientReturned.Measure.Should().Be(fakeIngredientOne.Measure);
        ingredientReturned.RecipeId.Should().Be(fakeIngredientOne.RecipeId);

        ingredientCreated.Name.Should().Be(fakeIngredientOne.Name);
        ingredientCreated.Quantity.Should().Be(fakeIngredientOne.Quantity);
        ingredientCreated.ExpiresOn.Should().BeCloseTo((DateTime)fakeIngredientOne.ExpiresOn, 1.Seconds());
        ingredientCreated.Measure.Should().Be(fakeIngredientOne.Measure);
        ingredientCreated.RecipeId.Should().Be(fakeIngredientOne.RecipeId);
    }
}