using Blog.Infrastructure.ConfigurationSections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace Blog.Infrastructure.EntityTypeConfigurations;

public class IdentityUserRoleEntityTypeConfigurations(IConfiguration _configuration) : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        var adminUser = new DefaultUser();
        _configuration.GetSection("AdminUser").Bind(adminUser);

        var publicUser = new DefaultUser();
        _configuration.GetSection("PublicUser").Bind(publicUser);

        var adminRole = new DefaultRole();
        _configuration.GetSection("AdminRole").Bind(adminRole);

        var publicRole = new DefaultRole();
        _configuration.GetSection("PublicRole").Bind(publicRole);

        builder.HasData(
            new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id,
            },
            new IdentityUserRole<string>
            {
                UserId = publicUser.Id,
                RoleId = publicRole.Id,
            }
        );
    }
}
