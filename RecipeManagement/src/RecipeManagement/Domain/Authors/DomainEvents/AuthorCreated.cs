namespace RecipeManagement.Domain.Authors.DomainEvents;

public sealed class AuthorCreated : DomainEvent
{
    public Author Author { get; set; } 
}
            