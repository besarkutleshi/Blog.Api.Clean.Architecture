using Blog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Seeds;

public static class SeedDb
{
    private readonly static string _adminUserId = "3fa85f64-5717-4562-b3fc-2c963f66afa4";
    private readonly static string _publicUserId = "3fa85f64-5717-4562-b3fc-2c963f66afa5";
    private readonly static string _adminRoleId = "3fa85f64-5717-4562-b3fc-2c963f66afa2";
    private readonly static string _publicRoleId = "3fa85f64-5717-4562-b3fc-2c963f66afa3";

    public static async Task CreateIdentityInstances(IServiceScope serviceScope)
    {
        try
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var adminRole = await dbContext.Roles.AsNoTracking().Select(x => new IdentityRole { Id = x.Id }).FirstOrDefaultAsync(x => x.Id == _adminRoleId);
            if (adminRole is null)
            {
                var identityAdminRole = new IdentityRole
                {
                    Id = _adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                };

                var result = await roleManager.CreateAsync(identityAdminRole);
                if (!result.Succeeded)
                    return;
            }

            var publicRole = await dbContext.Roles.AsNoTracking().Select(x => new IdentityRole { Id = x.Id }).FirstOrDefaultAsync(x => x.Id == _publicRoleId);
            if (publicRole is null)
            {
                var identityPublicRole = new IdentityRole
                {
                    Id = _publicRoleId,
                    Name = "Public",
                    NormalizedName = "PUBLIC"
                };

                var result = await roleManager.CreateAsync(identityPublicRole);
                if (!result.Succeeded)
                    return;
            }

            var adminUser = await dbContext.Users.AsNoTracking().Select(x => new IdentityUser { Id = x.Id }).FirstOrDefaultAsync(x => x.Id == _adminUserId);
            if (adminUser is null)
            {
                var identityAdminUser = new IdentityUser
                {
                    Id = _adminUserId,
                    UserName = "besarkutleshi",
                    Email = "besarkutleshi@outlook.com",
                    PhoneNumber = "1234567890",
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(identityAdminUser, "Besar.123");
                if (!result.Succeeded)
                    return;

                await userManager.AddToRoleAsync(identityAdminUser, "Admin");
            }

            var publicUser = await dbContext.Users.AsNoTracking().Select(x => new IdentityUser { Id = x.Id }).FirstOrDefaultAsync(x => x.Id == _publicUserId);
            if (publicUser is null)
            {
                var identityPublicUser = new IdentityUser
                {
                    Id = _publicUserId,
                    UserName = "filanfisteku",
                    Email = "filanfisteku@outlook.com",
                    PhoneNumber = "1234567899",
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(identityPublicUser, "Filani.123");
                if (!result.Succeeded)
                    return;

                await userManager.AddToRoleAsync(identityPublicUser, "Public");
            }

            await dbContext.SaveChangesAsync();
        }
        catch
        {
            throw;
        }
    }
}
