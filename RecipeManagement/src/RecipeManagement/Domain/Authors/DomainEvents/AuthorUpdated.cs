namespace RecipeManagement.Domain.Authors.DomainEvents;

public sealed class AuthorUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            