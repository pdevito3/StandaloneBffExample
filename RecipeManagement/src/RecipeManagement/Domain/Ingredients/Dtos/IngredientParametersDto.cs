namespace RecipeManagement.Domain.Ingredients.Dtos;

using SharedKernel.Dtos;

public sealed class IngredientParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
