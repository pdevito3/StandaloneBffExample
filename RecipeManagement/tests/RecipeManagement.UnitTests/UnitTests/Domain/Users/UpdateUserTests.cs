namespace RecipeManagement.UnitTests.UnitTests.Domain.Users;

using RecipeManagement.Domain.Users.DomainEvents;
using RecipeManagement.Domain;
using RecipeManagement.Domain.Users;
using RecipeManagement.Wrappers;
using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.User;
using SharedKernel.Exceptions;
using Bogus;
using FluentAssertions;
using NUnit.Framework;
using ValidationException = SharedKernel.Exceptions.ValidationException;

[Parallelizable]
public class UpdateUserTests
{
    private readonly Faker _faker;

    public UpdateUserTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_user()
    {
        // Arrange
        var fakeUser = FakeUser.Generate();
        var updatedUser = new FakeUserForUpdateDto().Generate();
        
        // Act
        fakeUser.Update(updatedUser);

        // Assert
        fakeUser.Identifier.Should().Be(updatedUser.Identifier);
        fakeUser.FirstName.Should().Be(updatedUser.FirstName);
        fakeUser.LastName.Should().Be(updatedUser.LastName);
        fakeUser.Email.Value.Should().Be(updatedUser.Email);
        fakeUser.Username.Should().Be(updatedUser.Username);
    }
    
    [Test]
    public void can_NOT_update_user_without_identifier()
    {
        // Arrange
        var fakeUser = FakeUser.Generate();
        var updatedUser = new FakeUserForUpdateDto().Generate();
        updatedUser.Identifier = null;
        var newUser = () => fakeUser.Update(updatedUser);

        // Act + Assert
        newUser.Should().Throw<ValidationException>();
    }
    
    [Test]
    public void can_NOT_update_user_with_whitespace_identifier()
    {
        // Arrange
        var fakeUser = FakeUser.Generate();
        var updatedUser = new FakeUserForUpdateDto().Generate();
        updatedUser.Identifier = " ";
        var newUser = () => fakeUser.Update(updatedUser);

        // Act + Assert
        newUser.Should().Throw<ValidationException>();
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeUser = FakeUser.Generate();
        var updatedUser = new FakeUserForUpdateDto().Generate();
        fakeUser.DomainEvents.Clear();
        
        // Act
        fakeUser.Update(updatedUser);

        // Assert
        fakeUser.DomainEvents.Count.Should().Be(1);
        fakeUser.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserUpdated));
    }
}