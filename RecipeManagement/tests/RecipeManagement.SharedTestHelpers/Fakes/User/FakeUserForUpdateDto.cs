namespace RecipeManagement.SharedTestHelpers.Fakes.User;

using AutoBogus;
using RecipeManagement.Domain;
using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Domain.Roles;

public sealed class FakeUserForUpdateDto : AutoFaker<UserForUpdateDto>
{
    public FakeUserForUpdateDto()
    {
        RuleFor(u => u.Email, f => f.Person.Email);
    }
}