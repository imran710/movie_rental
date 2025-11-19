using Core.Features.PermissionManagement.Permissions;

namespace Core.Features.PermissionManagement.Common.System;

public static class SubscriptionPermissions
{
    public static readonly Permission AddRecipe = SystemPermissions.Recipe.AddRecipe;
    public static readonly Permission DetectMealImage = SystemPermissions.Meal.DetectMealImage;
    public static readonly Permission AddMeal = SystemPermissions.Meal.AddMeal;
    public static readonly Permission JoinTribe = SystemPermissions.Tribe.JoinTribe;

    public static readonly IReadOnlyList<Permission> AllPermissions =
    [
        AddRecipe,
        DetectMealImage,
        AddMeal,
        JoinTribe
    ];
}
