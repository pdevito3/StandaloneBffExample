namespace RecipeManagement.Domain;

using System.Reflection;

public static class Permissions
{
    // Permissions marker - do not delete this comment
    public const string CanDeleteRecipes = nameof(CanDeleteRecipes);
    public const string CanUpdateRecipes = nameof(CanUpdateRecipes);
    public const string CanAddRecipes = nameof(CanAddRecipes);
    public const string CanReadRecipes = nameof(CanReadRecipes);
    public const string CanDeleteUsers = nameof(CanDeleteUsers);
    public const string CanUpdateUsers = nameof(CanUpdateUsers);
    public const string CanAddUsers = nameof(CanAddUsers);
    public const string CanReadUsers = nameof(CanReadUsers);
    public const string CanDeleteRolePermissions = nameof(CanDeleteRolePermissions);
    public const string CanUpdateRolePermissions = nameof(CanUpdateRolePermissions);
    public const string CanAddRolePermissions = nameof(CanAddRolePermissions);
    public const string CanReadRolePermissions = nameof(CanReadRolePermissions);
    public const string CanRemoveUserRoles = nameof(CanRemoveUserRoles);
    public const string CanAddUserRoles = nameof(CanAddUserRoles);
    public const string CanGetRoles = nameof(CanGetRoles);
    public const string CanGetPermissions = nameof(CanGetPermissions);
    
    public static List<string> List()
    {
        return typeof(Permissions)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
            .Select(x => (string)x.GetRawConstantValue())
            .ToList();
    }
}
