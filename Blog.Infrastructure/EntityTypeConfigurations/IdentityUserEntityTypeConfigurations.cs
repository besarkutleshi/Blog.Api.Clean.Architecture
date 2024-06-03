using Blog.Infrastructure.ConfigurationSections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace Blog.Infrastructure.EntityTypeConfigurations;

public class IdentityUserEntityTypeConfigurations(IConfiguration _configuration) : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        var adminUser = new DefaultUser();
        _configuration.GetSection("AdminUser").Bind(adminUser);

        var publicUser = new DefaultUser();
        _configuration.GetSection("PublicUser").Bind(publicUser);

        builder.HasData(
            new IdentityUser
            {
                Id = adminUser.Id,
                Email = adminUser.Email,
                NormalizedEmail = adminUser.Email.Normalize(),
                PasswordHash = adminUser.PasswordHash,
                UserName = adminUser.Username,
                NormalizedUserName = adminUser.Username.Normalize(),
                PhoneNumber = adminUser.PhoneNumber,
                EmailConfirmed = adminUser.EmailConfirmed,
            },
            new IdentityUser
            {
                Id = publicUser.Id,
                Email = publicUser.Email,
                NormalizedEmail = publicUser.Email.Normalize(),
                PasswordHash = publicUser.PasswordHash,
                UserName = publicUser.Username,
                NormalizedUserName = publicUser.Username.Normalize(),
                PhoneNumber = publicUser.PhoneNumber,
                EmailConfirmed = publicUser.EmailConfirmed,
            }
        );
    }
}
