namespace RecipeManagement.UnitTests.UnitTests.Domain.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateRecipeTests
{
    private readonly Faker _faker;

    public UpdateRecipeTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_recipe()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate();
        var updatedRecipe = new FakeRecipeForUpdateDto().Generate();
        
        // Act
        fakeRecipe.Update(updatedRecipe);

        // Assert
        fakeRecipe.Title.Should().Be(updatedRecipe.Title);
        fakeRecipe.Visibility.Should().Be(updatedRecipe.Visibility);
        fakeRecipe.Directions.Should().Be(updatedRecipe.Directions);
        fakeRecipe.Rating.Should().Be(updatedRecipe.Rating);
        fakeRecipe.DateOfOrigin.Should().Be(updatedRecipe.DateOfOrigin);
        fakeRecipe.HaveMadeItMyself.Should().Be(updatedRecipe.HaveMadeItMyself);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate();
        var updatedRecipe = new FakeRecipeForUpdateDto().Generate();
        fakeRecipe.DomainEvents.Clear();
        
        // Act
        fakeRecipe.Update(updatedRecipe);

        // Assert
        fakeRecipe.DomainEvents.Count.Should().Be(1);
        fakeRecipe.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(RecipeUpdated));
    }
}