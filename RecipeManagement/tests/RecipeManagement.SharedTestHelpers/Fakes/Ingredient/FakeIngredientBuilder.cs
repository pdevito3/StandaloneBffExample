namespace RecipeManagement.SharedTestHelpers.Fakes.Ingredient;

using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Dtos;

public class FakeIngredientBuilder
{
    private IngredientForCreationDto _creationData = new FakeIngredientForCreationDto().Generate();

    public FakeIngredientBuilder WithDto(IngredientForCreationDto dto)
    {
        _creationData = dto;
        return this;
    }
    
    public Ingredient Build()
    {
        var result = Ingredient.Create(_creationData);
        return result;
    }
}