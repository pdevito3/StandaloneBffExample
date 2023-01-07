namespace RecipeManagement.SharedTestHelpers.Fakes.RolePermission;

using AutoBogus;
using RecipeManagement.Domain;
using RecipeManagement.Domain.RolePermissions.Dtos;
using RecipeManagement.Domain.Roles;

public sealed class FakeRolePermissionForCreationDto : AutoFaker<RolePermissionForCreationDto>
{
    public FakeRolePermissionForCreationDto()
    {
        RuleFor(rp => rp.Permission, f => f.PickRandom(Permissions.List()));
        RuleFor(rp => rp.Role, f => f.PickRandom(Role.ListNames()));
    }
}