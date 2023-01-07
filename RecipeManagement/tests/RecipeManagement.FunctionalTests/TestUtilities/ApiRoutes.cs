namespace RecipeManagement.FunctionalTests.TestUtilities;
public class ApiRoutes
{
    public const string Base = "api";
    public const string Health = Base + "/health";

    // new api route marker - do not delete

    public static class Ingredients
    {
        public static string GetList => $"{Base}/ingredients";
        public static string GetRecord(Guid id) => $"{Base}/ingredients/{id}";
        public static string Delete(Guid id) => $"{Base}/ingredients/{id}";
        public static string Put(Guid id) => $"{Base}/ingredients/{id}";
        public static string Create => $"{Base}/ingredients";
        public static string CreateBatch => $"{Base}/ingredients/batch";
    }

    public static class Authors
    {
        public static string GetList => $"{Base}/authors";
        public static string GetRecord(Guid id) => $"{Base}/authors/{id}";
        public static string Delete(Guid id) => $"{Base}/authors/{id}";
        public static string Put(Guid id) => $"{Base}/authors/{id}";
        public static string Create => $"{Base}/authors";
        public static string CreateBatch => $"{Base}/authors/batch";
    }

    public static class Recipes
    {
        public static string GetList => $"{Base}/recipes";
        public static string GetRecord(Guid id) => $"{Base}/recipes/{id}";
        public static string Delete(Guid id) => $"{Base}/recipes/{id}";
        public static string Put(Guid id) => $"{Base}/recipes/{id}";
        public static string Create => $"{Base}/recipes";
        public static string CreateBatch => $"{Base}/recipes/batch";
    }

    public static class Users
    {
        public static string GetList => $"{Base}/users";
        public static string GetRecord(Guid id) => $"{Base}/users/{id}";
        public static string Delete(Guid id) => $"{Base}/users/{id}";
        public static string Put(Guid id) => $"{Base}/users/{id}";
        public static string Create => $"{Base}/users";
        public static string CreateBatch => $"{Base}/users/batch";
        public static string AddRole(Guid id) => $"{Base}/users/{id}/addRole";
        public static string RemoveRole(Guid id) => $"{Base}/users/{id}/removeRole";
    }

    public static class RolePermissions
    {
        public static string GetList => $"{Base}/rolePermissions";
        public static string GetRecord(Guid id) => $"{Base}/rolePermissions/{id}";
        public static string Delete(Guid id) => $"{Base}/rolePermissions/{id}";
        public static string Put(Guid id) => $"{Base}/rolePermissions/{id}";
        public static string Create => $"{Base}/rolePermissions";
        public static string CreateBatch => $"{Base}/rolePermissions/batch";
    }
}
