using Blog.Infrastructure.ConfigurationSections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace Blog.Infrastructure.EntityTypeConfigurations;

public class IdentityRoleEntityTypeConfigurations(IConfiguration _configuration) : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        var adminRole = new DefaultRole();
        _configuration.GetSection("AdminRole").Bind(adminRole);

        var publicRole = new DefaultRole();
        _configuration.GetSection("PublicRole").Bind(publicRole);

        builder.HasData(
            new IdentityRole
            {
                Id = adminRole.Id,
                Name = adminRole.Name,
                NormalizedName = adminRole.Name.Normalize(),
            },
            new IdentityRole
            {
                Id = publicRole.Id,
                Name= publicRole.Name,
                NormalizedName = publicRole.Name.Normalize(),
            }
        );
    }
}
