namespace RecipeManagement.Domain.Authors.Dtos;

public sealed class AuthorDto 
{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RecipeId { get; set; }
}
