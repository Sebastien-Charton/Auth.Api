using Auth.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Api.Infrastructure.Configurations;

public class RefreshTokenConfiguration: IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.Created)
            .IsRequired();
        
        builder
            .Property(x => x.CreatedBy)
            .IsRequired();
        
        builder
            .Property(x => x.Token)
            .IsRequired();

        builder
            .Property(x => x.Expires)
            .IsRequired();

        builder
            .Property(x => x.UserId)
            .IsRequired();
        
        builder
            .Property(x => x.RevokedBy)
            .IsRequired(false);
        
        builder
            .Property(x => x.Revoked)
            .IsRequired(false);
        
    }
}
