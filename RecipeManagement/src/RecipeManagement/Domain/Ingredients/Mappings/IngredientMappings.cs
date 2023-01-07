namespace RecipeManagement.Domain.Ingredients.Mappings;

using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients;
using Mapster;

public sealed class IngredientMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Ingredient, IngredientDto>();
        config.NewConfig<IngredientForCreationDto, Ingredient>()
            .TwoWays();
        config.NewConfig<IngredientForUpdateDto, Ingredient>()
            .TwoWays();
    }
}