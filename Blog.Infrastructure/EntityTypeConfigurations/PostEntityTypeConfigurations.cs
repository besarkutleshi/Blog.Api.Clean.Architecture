using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.EntityTypeConfigurations;
public sealed class PostEntityTypeConfigurations : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(x => x.Id).HasName("Posts_id_pk");

        builder.Property(x => x.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.FriendlyUrl)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(x => x.CreatedBy)
            .IsRequired(false);

        builder.Property(x => x.DateCreated);

        builder.HasIndex(x => x.FriendlyUrl)
            .IsUnique();

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedBy);
    }
}
