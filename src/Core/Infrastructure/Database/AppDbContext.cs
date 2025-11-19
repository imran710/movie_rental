using System.Reflection;

using Core.Features.Auth.DeviceInformations;
using Core.Features.Auth.LoginActivities;
using Core.Features.Auth.OtpVerifications;
using Core.Features.Auth.RefreshTokens;
using Core.Features.PermissionManagement.Permissions;
using Core.Features.PermissionManagement.RolePermissions;
using Core.Features.PermissionManagement.Roles;
using Core.Features.PermissionManagement.UserRoles;
using Core.Features.Rentals.Entity;
using Core.Features.Users.Entity;

using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<DeviceInformation> DeviceInformation => Set<DeviceInformation>();
    public DbSet<LoginActivity> LoginActivities => Set<LoginActivity>();
    public DbSet<OtpVerification> OtpVerifications => Set<OtpVerification>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
     
    public DbSet<DashboardFile> DashboardFiles => Set<DashboardFile>(); 
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<RentalItem> RentalItems => Set<RentalItem>();
    public DbSet<RentalE> Rentals => Set<RentalE>();

     
     

}
