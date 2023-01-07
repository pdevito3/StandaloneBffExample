namespace RecipeManagement.UnitTests.UnitTests.Domain.Users;

using RecipeManagement.SharedTestHelpers.Fakes.User;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Roles;
using RecipeManagement.Domain.Users.DomainEvents;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

[Parallelizable]
public class CreateUserRoleTests
{
    private readonly Faker _faker;

    public CreateUserRoleTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_userRole()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var role = _faker.PickRandom(Role.ListNames());
        
        // Act
        var fakeUserRole = UserRole.Create(userId, new Role(role));

        // Assert
        fakeUserRole.UserId.Should().Be(userId);
        fakeUserRole.Role.Should().Be(new Role(role));
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var role = _faker.PickRandom(Role.ListNames());
        
        // Act
        var fakeUserRole = UserRole.Create(userId, new Role(role));

        // Assert
        fakeUserRole.DomainEvents.Count.Should().Be(1);
        fakeUserRole.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserRolesUpdated));
    }
}