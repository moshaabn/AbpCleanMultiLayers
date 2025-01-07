using MoShaabn.CleanArch.Entities.Users;
using MoShaabn.CleanArch.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace MoShaabn.CleanArch.EntityConfigurations;

internal class OtpConfiguration : IEntityTypeConfiguration<Otp>
{
    public void Configure(EntityTypeBuilder<Otp> builder)
    {
        builder.HasOne(x => x.User)
            .WithOne(x => x.Otp)
            .HasForeignKey<Otp>(x => x.UserId).IsRequired();
            

    }
}