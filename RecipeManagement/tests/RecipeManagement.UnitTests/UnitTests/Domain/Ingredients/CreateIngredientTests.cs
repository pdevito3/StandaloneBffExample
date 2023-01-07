namespace RecipeManagement.UnitTests.UnitTests.Domain.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class CreateIngredientTests
{
    private readonly Faker _faker;

    public CreateIngredientTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_ingredient()
    {
        // Arrange
        var ingredientToCreate = new FakeIngredientForCreationDto().Generate();
        
        // Act
        var fakeIngredient = Ingredient.Create(ingredientToCreate);

        // Assert
        fakeIngredient.Name.Should().Be(ingredientToCreate.Name);
        fakeIngredient.Quantity.Should().Be(ingredientToCreate.Quantity);
        fakeIngredient.ExpiresOn.Should().BeCloseTo((DateTime)ingredientToCreate.ExpiresOn, 1.Seconds());
        fakeIngredient.Measure.Should().Be(ingredientToCreate.Measure);
        fakeIngredient.RecipeId.Should().Be(ingredientToCreate.RecipeId);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeIngredient = FakeIngredient.Generate();

        // Assert
        fakeIngredient.DomainEvents.Count.Should().Be(1);
        fakeIngredient.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(IngredientCreated));
    }
}