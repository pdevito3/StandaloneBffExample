namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Ingredients.Features;
using SharedKernel.Exceptions;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class AddListIngredientCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_ingredient_list_to_db()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipe);
        var fakeIngredientOne = new FakeIngredientForCreationDto().Generate();
        var fakeIngredientTwo = new FakeIngredientForCreationDto().Generate();

        // Act
        var command = new AddIngredientList.AddIngredientListCommand(new List<IngredientForCreationDto>() {fakeIngredientOne, fakeIngredientTwo}, fakeRecipe.Id);
        var ingredientReturned = await SendAsync(command);
        var firstReturned = ingredientReturned.FirstOrDefault();
        var secondReturned = ingredientReturned.Skip(1).FirstOrDefault();

        var ingredientDb = await ExecuteDbContextAsync(db => db.Ingredients
            .Where(x => x.Id == firstReturned.Id || x.Id == secondReturned.Id)
            .ToListAsync());
        var firstDbRecord = ingredientDb.FirstOrDefault(x => x.Id == firstReturned.Id);
        var secondDbRecord = ingredientDb.FirstOrDefault(x => x.Id == secondReturned.Id);

        // Assert
        firstReturned.Name.Should().Be(fakeIngredientOne.Name);
        secondReturned.Name.Should().Be(fakeIngredientTwo.Name);
        firstReturned.Quantity.Should().Be(fakeIngredientOne.Quantity);
        secondReturned.Quantity.Should().Be(fakeIngredientTwo.Quantity);
        firstReturned.ExpiresOn.Should().BeCloseTo((DateTime)fakeIngredientOne.ExpiresOn, 1.Seconds());
        secondReturned.ExpiresOn.Should().BeCloseTo((DateTime)fakeIngredientTwo.ExpiresOn, 1.Seconds());
        firstReturned.Measure.Should().Be(fakeIngredientOne.Measure);
        secondReturned.Measure.Should().Be(fakeIngredientTwo.Measure);
        firstReturned.RecipeId.Should().Be(fakeIngredientOne.RecipeId);
        secondReturned.RecipeId.Should().Be(fakeIngredientTwo.RecipeId);

        firstDbRecord.Name.Should().Be(fakeIngredientOne.Name);
        secondDbRecord.Name.Should().Be(fakeIngredientTwo.Name);
        firstDbRecord.Quantity.Should().Be(fakeIngredientOne.Quantity);
        secondDbRecord.Quantity.Should().Be(fakeIngredientTwo.Quantity);
        firstDbRecord.ExpiresOn.Should().BeCloseTo((DateTime)fakeIngredientOne.ExpiresOn, 1.Seconds());
        secondDbRecord.ExpiresOn.Should().BeCloseTo((DateTime)fakeIngredientTwo.ExpiresOn, 1.Seconds());
        firstDbRecord.Measure.Should().Be(fakeIngredientOne.Measure);
        secondDbRecord.Measure.Should().Be(fakeIngredientTwo.Measure);
        firstDbRecord.RecipeId.Should().Be(fakeIngredientOne.RecipeId);
        secondDbRecord.RecipeId.Should().Be(fakeIngredientTwo.RecipeId);
    }
}