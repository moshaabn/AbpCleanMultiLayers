using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoShaabn.CleanArch.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MoShaabn.CleanArch.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ConfigureByConvention();

            builder.Property(x => x.Email)
                .IsRequired(false);

            builder.Property(x => x.PhoneNumber)
               .IsRequired(false);

            builder.Property(x => x.NormalizedEmail)
                .IsRequired(false);

            builder.HasMany(x => x.RefreshTokens).WithOne(x => x.User).HasForeignKey(x => x.UserId);

        }
    }
}