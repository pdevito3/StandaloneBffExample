namespace RecipeManagement.Domain.Recipes.Dtos;

public sealed class RecipeDto 
{
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Visibility { get; set; }
        public string Directions { get; set; }
        public int? Rating { get; set; }
        public DateOnly? DateOfOrigin { get; set; }
        public bool HaveMadeItMyself { get; set; }

}
