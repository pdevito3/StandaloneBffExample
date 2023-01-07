namespace RecipeManagement.Domain.Ingredients.Dtos;

public sealed class IngredientDto 
{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public DateTime? ExpiresOn { get; set; }
        public string Measure { get; set; }
        public Guid RecipeId { get; set; }
}
