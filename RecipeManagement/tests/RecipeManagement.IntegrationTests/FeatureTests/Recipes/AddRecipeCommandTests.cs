namespace RecipeManagement.IntegrationTests.FeatureTests.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using RecipeManagement.Domain.Recipes.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using RecipeManagement.SharedTestHelpers.Fakes.Author;

public class AddRecipeCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_recipe_to_db()
    {
        // Arrange
        var fakeRecipeOne = new FakeRecipeForCreationDto().Generate();

        // Act
        var command = new AddRecipe.Command(fakeRecipeOne);
        var recipeReturned = await SendAsync(command);
        var recipeCreated = await ExecuteDbContextAsync(db => db.Recipes
            .FirstOrDefaultAsync(r => r.Id == recipeReturned.Id));

        // Assert
        recipeReturned.Title.Should().Be(fakeRecipeOne.Title);
        recipeReturned.Visibility.Should().Be(fakeRecipeOne.Visibility);
        recipeReturned.Directions.Should().Be(fakeRecipeOne.Directions);
        recipeReturned.Rating.Should().Be(fakeRecipeOne.Rating);
        recipeReturned.DateOfOrigin.Should().Be(fakeRecipeOne.DateOfOrigin);
        recipeReturned.HaveMadeItMyself.Should().Be(fakeRecipeOne.HaveMadeItMyself);

        recipeCreated.Title.Should().Be(fakeRecipeOne.Title);
        recipeCreated.Visibility.Should().Be(fakeRecipeOne.Visibility);
        recipeCreated.Directions.Should().Be(fakeRecipeOne.Directions);
        recipeCreated.Rating.Should().Be(fakeRecipeOne.Rating);
        recipeCreated.DateOfOrigin.Should().Be(fakeRecipeOne.DateOfOrigin);
        recipeCreated.HaveMadeItMyself.Should().Be(fakeRecipeOne.HaveMadeItMyself);
    }
}