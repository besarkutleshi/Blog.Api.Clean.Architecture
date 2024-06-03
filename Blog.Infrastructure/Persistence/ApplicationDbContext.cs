using Blog.Domain.Entities;
using Blog.Infrastructure.EntityTypeConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog.Infrastructure.Persistence;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : IdentityDbContext(options)
{
    public virtual DbSet<Post> Posts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new PostEntityTypeConfigurations());
        builder.ApplyConfiguration(new IdentityUserEntityTypeConfigurations(configuration));
        builder.ApplyConfiguration(new IdentityRoleEntityTypeConfigurations(configuration));
        builder.ApplyConfiguration(new IdentityUserRoleEntityTypeConfigurations(configuration));

        base.OnModelCreating(builder);
    }
}
