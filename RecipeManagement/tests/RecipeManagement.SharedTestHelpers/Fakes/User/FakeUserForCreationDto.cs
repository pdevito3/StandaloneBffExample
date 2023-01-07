namespace RecipeManagement.SharedTestHelpers.Fakes.User;

using AutoBogus;
using RecipeManagement.Domain;
using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Domain.Roles;

public sealed class FakeUserForCreationDto : AutoFaker<UserForCreationDto>
{
    public FakeUserForCreationDto()
    {
        RuleFor(u => u.Email, f => f.Person.Email);
    }
}