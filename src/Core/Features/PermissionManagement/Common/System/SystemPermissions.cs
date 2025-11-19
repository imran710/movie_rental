using Core.Features.PermissionManagement.Permissions;

namespace Core.Features.PermissionManagement.Common.System;

public static class SystemPermissions
{
    public static class User
    {
        public static readonly Permission ViewUser = Permission.CreateSystemPermission("ViewUser", "View.User");
        public static readonly Permission CreateUser = Permission.CreateSystemPermission("CreateUser", "Create.User");
        public static readonly Permission UpdateUser = Permission.CreateSystemPermission("UpdateUser", "Update.User");
        public static readonly Permission DeleteUser = Permission.CreateSystemPermission("DeleteUser", "Delete.User");
    }

    public static class Role
    {
        public static readonly Permission ViewRole = Permission.CreateSystemPermission("ViewRole", "View.Role");
        public static readonly Permission CreateRole = Permission.CreateSystemPermission("CreateRole", "Create.Role");
        public static readonly Permission UpdateRole = Permission.CreateSystemPermission("UpdateRole", "Update.Role");
        public static readonly Permission DeleteRole = Permission.CreateSystemPermission("DeleteRole", "Delete.Role");
    }

    public static class Tribe
    {
        public static readonly Permission JoinTribe = Permission.CreateSystemPermission("JoinTribe", "Join.Tribe");
        public static readonly Permission CreateTribe = Permission.CreateSystemPermission("CreateTribe", "Create.Tribe");
        public static readonly Permission PostInTribe = Permission.CreateSystemPermission("PostInTribe", "Post.Tribe");
        public static readonly Permission CommentInTribe = Permission.CreateSystemPermission("CommentInTribe", "Comment.Tribe");
    }

    public static class Recipe
    {
        public static readonly Permission AddRecipe = Permission.CreateSystemPermission("AddRecipe", "Add.Recipe");
        public static readonly Permission ViewRecipe = Permission.CreateSystemPermission("ViewRecipe", "View.Recipe");
        public static readonly Permission AddRecipeInteraction = Permission.CreateSystemPermission("AddRecipeInteraction", "Add.RecipeInteraction");
    }

    public static class Meal
    {
        public static readonly Permission AddMeal = Permission.CreateSystemPermission("AddMeal", "Add.Meal");
        public static readonly Permission DetectMealImage = Permission.CreateSystemPermission("DetectMealImage", "Detect.MealImage");
    }

    public static class Subscription
    {
        public static readonly Permission CreateSubscription = Permission.CreateSystemPermission("CreateSubscription", "Create.Subscription");
    }

    public static readonly IReadOnlyList<Permission> AllPermissions =
    [
        User.ViewUser,
        User.CreateUser,
        User.UpdateUser,
        User.DeleteUser,

        Role.ViewRole,
        Role.CreateRole,
        Role.UpdateRole,
        Role.DeleteRole,

        Tribe.JoinTribe,
        Tribe.CreateTribe,
        Tribe.PostInTribe,
        Tribe.CommentInTribe,

        Recipe.AddRecipe,
        Recipe.ViewRecipe,
        Recipe.AddRecipeInteraction,

        Meal.AddMeal,
        Meal.DetectMealImage,

        Subscription.CreateSubscription
    ];
}
