namespace RecipeManagement.Domain.Ingredients.DomainEvents;

public sealed class IngredientCreated : DomainEvent
{
    public Ingredient Ingredient { get; set; } 
}
            