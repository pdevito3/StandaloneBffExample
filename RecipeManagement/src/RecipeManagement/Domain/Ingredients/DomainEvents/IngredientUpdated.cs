namespace RecipeManagement.Domain.Ingredients.DomainEvents;

public sealed class IngredientUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            